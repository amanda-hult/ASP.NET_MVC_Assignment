using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class DateOfBirthService(IDateOfBirthRepository dateOfBirthRepository) : IDateOfBirthService
{
    private readonly IDateOfBirthRepository _dateOfBirthRepository = dateOfBirthRepository;

    // CREATE
    public async Task<bool> CreateDateOfBirthAsync(DateOfBirthCreateModel model)
    {
        var createdDate = await _dateOfBirthRepository.CreateAsync(DateOfBirthFactory.Create(model));
        if (createdDate == null)
            return false;

        return true;
    }

    // READ
    public async Task<DateOfBirthEntity> GetDateOfBirthEntityByIdAsync(int id)
    {
        var dateEntity = await _dateOfBirthRepository.GetAsync(x => x.DateOfBirthId == id);

        if (dateEntity == null)
            throw new ArgumentNullException(nameof(dateEntity));

        return dateEntity;
    }

    // UPDATE

    // DELETE
}
