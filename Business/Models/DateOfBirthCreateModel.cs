using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class DateOfBirthCreateModel
{
    [Required]
    public int Date { get; set; }

    [Required]
    public string Month { get; set; } = null!;

    [Required]
    public int Year { get; set; }
}
