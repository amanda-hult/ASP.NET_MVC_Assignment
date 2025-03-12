using Business.Models;
using Data.Entities;

namespace Business.Interfaces;

public interface IDateOfBirthService
{
    Task<bool> CreateDateOfBirthAsync(DateOfBirthCreateModel model);
    Task<DateOfBirthEntity> GetDateOfBirthEntityByIdAsync(int id);
}