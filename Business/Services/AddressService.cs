//using Business.Factories;
//using Business.Interfaces;
//using Business.Models;
//using Data.Entities;
//using Data.Interfaces;

//namespace Business.Services;

//public class AddressService(IAddressRepository addressRepository) : IAddressService
//{
//    private readonly IAddressRepository _addressRepository = addressRepository;

//    // CREATE
//    public async Task<bool> CreateAddressAsync(AddressCreateModel model)
//    {
//        var createdAddress = await _addressRepository.CreateAsync(AddressFactory.Create(model));
//        if (createdAddress == null)
//            return false;

//        return true;
//    }

//    // READ
//    public async Task<AddressEntity> GetAddressEntityByIdAsync(int id)
//    {
//        var addressEntity = await _addressRepository.GetAsync(x => x.AddressId == id);

//        if (addressEntity == null)
//            throw new ArgumentNullException(nameof(addressEntity));

//        return addressEntity;
//    }

//    // UPDATE

//    // DELETE
//}
