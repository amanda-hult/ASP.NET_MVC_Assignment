using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class DateOfBirthRepository(DataContext context) : BaseRepository<DateOfBirthEntity>(context), IDateOfBirthRepository
{
}
