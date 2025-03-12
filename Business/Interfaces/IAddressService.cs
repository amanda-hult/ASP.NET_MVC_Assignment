using Business.Models;
using Data.Entities;

namespace Business.Interfaces;

public interface IAddressService
{
    Task<bool> CreateAddressAsync(AddressCreateModel model);
    Task<AddressEntity> GetAddressEntityByIdAsync(int id);
}