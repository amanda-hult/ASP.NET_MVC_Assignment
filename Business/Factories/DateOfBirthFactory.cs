using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class DateOfBirthFactory
{
    public static DateOfBirthEntity Create(DateOfBirthCreateModel model)
    {
        return new DateOfBirthEntity
        {
            Date = model.Date,
            Month = model.Month,
            Year = model.Year,
        };
    }
}
