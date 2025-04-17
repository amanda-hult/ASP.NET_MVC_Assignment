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

        return addressEntity;
    }
    #endregion

    // READ
    public async Task<AddressEntity> GetAddressEntityByIdAsync(int id)
    {
        var addressEntity = await _addressRepository.GetAsync(x => x.AddressId == id);

        if (addressEntity == null)
            throw new ArgumentNullException(nameof(addressEntity));

        return addressEntity;
    }


    #region Update
    public async Task<AddressEntity> UpdateAddressAsync(AddressEditModel model)
    {
        var updatedAddress = AddressFactory.Update(model);

        var result = await _addressRepository.UpdateAsync(x => x.AddressId == model.Id, updatedAddress);
        if (result == null)
            return null;

        return result;
    }
    #endregion

    // DELETE
}
