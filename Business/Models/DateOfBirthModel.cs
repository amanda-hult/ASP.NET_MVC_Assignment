namespace Business.Models;

public class DateOfBirthModel
{
    public int DateOfBirthId { get; set; }

    // -> DateTime ?
    public int Date { get; set; }

    public string Month { get; set; } = null!;

    public int Year { get; set; }
}
