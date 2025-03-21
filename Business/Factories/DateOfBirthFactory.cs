using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class DateOfBirthFactory
{
    public static DateOfBirthEntity Create(DateOfBirthCreateModel model)
    {
        return new DateOfBirthEntity
        {
            Date = model.SelectedDay,
            Month = model.SelectedMonth,
            Year = model.SelectedYear,
        };
    }
}
