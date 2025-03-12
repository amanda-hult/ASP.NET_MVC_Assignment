using System.Diagnostics;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;

namespace Business.Services;

public class UserService(IUserRepository userRepository, IProjectRepository projectRepository, IRoleService roleService, IAddressService addressService, IDateOfBirthService dateofbirthService) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IProjectRepository _projectRepository = projectRepository;

    private readonly IRoleService _roleService = roleService;
    private readonly IAddressService _addressService = addressService;
    private readonly IDateOfBirthService _dateofbirthService = dateofbirthService;

    //// CREATE
    //public async Task<bool> CreateUser(UserCreateModel model)
    //{
    //    // check if user already exists
    //    bool exists = await _userRepository.ExistsAsync(x => x.Email == model.Email);
    //    if (exists)
    //        return false;

    //    try
    //    {
    //        // get role
    //        var roleEntity = _roleService.GetRoleEntityByIdAsync(model.RoleId);

    //        // check existence/create address and dateofbirth
    //        var addressEntity = _addressService.GetAddressEntityByIdAsync(model.AddressId);
    //        if (addressEntity == null)
    //        {
    //            await _addressService.CreateAddressAsync(model.Address);
    //            var createdAddress = _addressService.GetAddressEntityByIdAsync(model.AddressId);
    //        }

    //        var dateEntity = _dateofbirthService.GetDateOfBirthEntityByIdAsync(model.DateOfBirthId);
    //        if (dateEntity == null)
    //        {
    //            await _dateofbirthService.CreateDateOfBirthAsync(model.DateOfBirth);
    //            var createdDate = _dateofbirthService.GetDateOfBirthEntityByIdAsync(model.DateOfBirthId);
    //        }



    //        var createdUser = await _userRepository.CreateAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine($"Error creating user: {ex.Message}");
    //    }

    //}

    //// READ
    //public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
    //{
    //    var users = (await _userRepository.GetAllAsync()).Select(UserFactory.Create).ToList();

    //    if (users.Count == 0)
    //        throw new ArgumentNullException(nameof(users));

    //    return users;
    //}

    // UPDATE

    // DELETE
    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetAsync(x => x.UserId == id);
        if (user == null)
            return false;

        var exists = await _projectRepository.ExistsAsync(x => x.UserId == id);
        if (exists)
            return false;

        await _userRepository.DeleteAsync(x => x.UserId == id);
        return true;
    }
}
