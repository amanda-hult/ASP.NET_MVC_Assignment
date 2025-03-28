using System.Diagnostics;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class UserService(UserManager<UserEntity> userManager, IUserRepository userRepository, IAddressService addressService) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    //private readonly IProjectRepository _projectRepository = projectRepository;

    private readonly IAddressService _addressService = addressService;
    //private readonly IDateOfBirthService _dateofbirthService = dateofbirthService;


    private readonly UserManager<UserEntity> _userManager = userManager;

    // CREATE
    public async Task<bool> CreateUserAsync(UserCreateModel model)
    {
        // check if user already exists
        bool exists = await _userRepository.ExistsAsync(x => x.Email == model.Email);
        if (exists)
            return false;

        // begin transaction

        try
        {
            var createdAddress = await _addressService.CreateAddressAsync(model.Address);
            if (createdAddress == null)
            {
                return false;
            }

            var userEntity = UserFactory.Create(model, createdAddress);
            var result = await _userManager.CreateAsync(userEntity, model.Password);

            return result.Succeeded;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating user: {ex.Message}");
            return false;
        }

    }

    // READ
    public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
    {
        var list = await _userManager.Users.ToListAsync();
        var users = list.Select(UserFactory.CreateBasic).ToList();
        return users;
        //var users = (await _userRepository.GetAllAsync()).Select(UserFactory.Create).ToList();

        //if (users.Count == 0)
        //    throw new ArgumentNullException(nameof(users));

        //return users;
    }

    public async Task<IEnumerable<UserModel>> GetUsersByIdAsync(List<string> ids)
    {
        var list = await _userRepository.GetUsersByIdAsync(ids);
        if (list == null)
            return null;

        var users = list.Select(UserFactory.CreateBasic).ToList();

        return users;
    }

    public async Task<List<UserEntity>> GetUserEntitiesByIdAsync(List<string> ids)
    {
        var users = await _userRepository.GetUsersByIdAsync(ids);
        if (users == null)
            return null;

        return users.ToList();
    }

    // UPDATE

    // DELETE
    //public async Task<bool> DeleteUserAsync(int id)
    //{
    //    var user = await _userRepository.GetAsync(x => x.UserId == id);
    //    if (user == null)
    //        return false;

    //    var exists = await _projectRepository.ExistsAsync(x => x.UserId == id);
    //    if (exists)
    //        return false;

    //    await _userRepository.DeleteAsync(x => x.UserId == id);
    //    return true;
    //}
}
