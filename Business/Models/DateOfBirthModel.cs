namespace Business.Models;

public class DateOfBirthModel
{
    public int? DateOfBirthId { get; set; }

    // -> DateTime ?
    public int? Date { get; set; }

    public int? Month { get; set; } = null!;

    public int? Year { get; set; }
}
