using Business.Factories;
using Business.Interfaces;
using Business.Models.Users;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class AddressService(IAddressRepository addressRepository) : IAddressService
{
    private readonly IAddressRepository _addressRepository = addressRepository;

    #region Create
    public async Task<AddressEntity> CreateAddressAsync(AddressCreateModel model)
    {
        var addressEntity = await _addressRepository.CreateAsync(AddressFactory.Create(model));
        if (addressEntity == null)
            return null!;

        await _addressRepository.SaveAsync();
        return addressEntity;
    }

    public async Task<AddressEntity> CreateNewAddressAsync(AddressEditModel model)
    {
        var addressEntity = await _addressRepository.CreateAsync(AddressFactory.Create(model));
        if (addressEntity == null)
            return null!;

        await _addressRepository.SaveAsync();
        return addressEntity;
    }
    #endregion

    #region Read
    public async Task<AddressEntity> GetAddressEntityByIdAsync(int id)
    {
        var addressEntity = await _addressRepository.GetAsync(x => x.AddressId == id);

        if (addressEntity == null)
            throw new ArgumentNullException(nameof(addressEntity));

        return addressEntity;
    }
    #endregion

    #region Update
    public async Task<AddressEntity> UpdateAddressAsync(AddressEditModel model)
    {
        var existingAddress = await _addressRepository.GetAsync(x => x.AddressId == model.Id);
        if (existingAddress == null)
            return null!;

        AddressFactory.Update(model, existingAddress);

        await _addressRepository.SaveAsync();
        return existingAddress;
    }
    #endregion

    #region Delete
    public async Task<int> DeleteAddressAsync(int? id)
    {
        bool exists = await _addressRepository.ExistsAsync(x => x.AddressId == id);
        if (!exists)
            return 404;

        bool result = await _addressRepository.DeleteAsync(x => x.AddressId == id);
        if (!result)
            return 500;

        return 204;
    }
    #endregion
}
