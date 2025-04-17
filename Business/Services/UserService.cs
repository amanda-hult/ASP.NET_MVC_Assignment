using System.Diagnostics;
using Business.Factories;
using Business.Interfaces;
using Business.Models.Users;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
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

        var users = list.Select(UserFactory.CreateBasic).ToList();
        return users;
        //var users = (await _userRepository.GetAllAsync()).Select(UserFactory.Create).ToList();

        //if (users.Count == 0)
        //    throw new ArgumentNullException(nameof(users));

        //return users;
    }

    // byt eventuellt ut denna metod mot nedan
    public async Task<IEnumerable<UserModel>> GetUsersByIdAsync(List<string> ids)
    {
        var list = await _userRepository.GetUsersByIdAsync(ids);
        if (list == null)
            return null;

        var users = list.Select(UserFactory.CreateBasic).ToList();

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

    public async Task<List<UserEntity>> GetUserEntitiesByIdAsync(List<string> ids)
    {
        var users = await _userRepository.GetUsersByIdAsync(ids);
        if (users == null)
            return null;

        return users.ToList();
    }

    //public async Task<UserModel> GetUserAsync(string id)
    //{
    //    var userEntity = await _userManager.FindByIdAsync(id);
    //    if (userEntity == null)
    //        return null;

    //    var userModel = UserFactory.CreateBasic(userEntity);
    //    return userModel;
    //}
    // UPDATE



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

            var updatedEntity = UserFactory.CreateUpdated(model, existingEntity);

            var result = await _userManager.UpdateAsync(updatedEntity);
            if (!result.Succeeded)
            {
                return 500;
            }

            var updatedAddress = await _addressService.UpdateAddressAsync(model.Address);
            if (updatedAddress == null)
            {
                Debug.WriteLine("Address update failed, but user was updated.");
                return 500;
            }

            updatedEntity.Address = updatedAddress;

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
    //public async Task<int> DeleteUserAsync(string id)
    //{

        // delete address

        //var user = await _userManager.FindByIdAsync(id);
        //if (user == null)
        //    return 404;

        //bool existsInProject = await _projectRepository.UserExistsInProject(id);
        //if (existsInProject)
        //    return 409;

        //var result = await _userManager.DeleteAsync(user);
        //if (!result.Succeeded)
        //    return 500;

        //return 201;
    //}
    #endregion
}
