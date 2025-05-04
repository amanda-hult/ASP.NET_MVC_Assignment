using System.Diagnostics;
using Business.Factories;
using Business.Interfaces;
using Business.Models.Users;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class UserService(UserManager<UserEntity> userManager, IUserRepository userRepository, IAddressService addressService, IProjectRepository projectRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IAddressService _addressService = addressService;
    private readonly UserManager<UserEntity> _userManager = userManager;

    #region Create
    public async Task<int> CreateUserAsync(UserCreateModel model)
    {
        // check if user already exists
        bool exists = await _userRepository.ExistsAsync(x => x.Email == model.Email);
        if (exists)
            return 409;

        try
        {
            var userEntity = UserFactory.Create(model);
            var result = await _userManager.CreateAsync(userEntity, model.Password);

            if (!result.Succeeded)
            {
                return 500;
            }

            var createdAddress = await _addressService.CreateAddressAsync(model.Address);
            if (createdAddress == null)
            {
                await _userManager.DeleteAsync(userEntity);
                return 500;
            }

            userEntity.Address = createdAddress;

            await _userRepository.SaveAsync();
            return 201;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating user: {ex.Message}");
            return 500;
        }

    }
    #endregion

    #region Read
    public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
    {
        var list = await _userManager.Users
                    .Include(u => u.Address)
                    .ToListAsync();

        var users = list.Select(UserFactory.Create).ToList();
        return users;
    }

    public async Task<IEnumerable<BasicUserModel>> GetBasicUsersByStringAsync(string term)
    {
        var list = await _userRepository.GetUsersByStringAsync(term);
        if (list == null)
            return Enumerable.Empty<BasicUserModel>();

        var users = list.Select(UserFactory.CreateBasicUser).ToList();

        return users;
    }

    public async Task<IEnumerable<BasicUserModel>> GetBasicUsersByIdAsync(List<string> ids)
    {
        var list = await _userRepository.GetUsersByIdAsync(ids);
        if (list == null)
            return Enumerable.Empty<BasicUserModel>();

        var users = list.Select(UserFactory.CreateBasicUser).ToList();

        return users;
    }

    #endregion

    #region Update
    public async Task<int> UpdateUserAsync(UserEditModel model)
    {
        // check if user already exists
        bool exists = await _userRepository.ExistsAsync(x => x.Email == model.Email && x.Id != model.Id);
        if (exists)
            return 409;

        try
        {
            var existingEntity = await _userManager.FindByIdAsync(model.Id);
            if (existingEntity == null)
                return 404;

            UserFactory.Update(model, existingEntity);

            var result = await _userManager.UpdateAsync(existingEntity);
            if (!result.Succeeded)
            {
                return 500;
            }

            if (existingEntity.Address == null || existingEntity.AddressId == null)
            {
                var newAddress = await _addressService.CreateNewAddressAsync(model.Address);
                existingEntity.Address = newAddress;
            }
            else
            {
                var updatedAddress = await _addressService.UpdateAddressAsync(model.Address);
                existingEntity.Address = updatedAddress;
            }

            return 200;
        }

        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating user: {ex.Message}");
            return 500;
        }

    }
    #endregion

    #region Delete
    public async Task<int> DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return 404;

        bool existsInProject = await _projectRepository.UserExistsInProject(id);
        if (existsInProject)
            return 409;

        await _userRepository.BeginTransactionAsync();
        try
        {
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return 500;

            if (user.Address != null || user.AddressId != null)
            {
                var addressUsedByOtherUsers = await _userRepository.ExistsAsync(x => x.AddressId == user.AddressId && x.Id != user.Id);
                if (!addressUsedByOtherUsers)
                {
                    await _addressService.DeleteAddressAsync(user.AddressId);
                }
            }
            await _userRepository.CommitTransactionAsync();
            return 204;
        }
        catch(Exception ex)
        {
            Debug.WriteLine($"Error deleting user: {ex.Message}");
            await _userRepository.RollbackTransactionAsync();
            return 500;
        }
    }
    #endregion
}
