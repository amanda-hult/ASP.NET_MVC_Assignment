using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class DateOfBirthCreateModel
{
    [Required(ErrorMessage = "Required")]
    public int? SelectedDay { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    public int? SelectedMonth { get; set; }


    [Required(ErrorMessage = "Required")]
    public int? SelectedYear { get; set; }


    public IEnumerable<int> Days { get; set; } = Enumerable.Range(1, 31).ToList();
    public IEnumerable<int> Months { get; set; } = Enumerable.Range(1, 12).ToList();
    public IEnumerable<int> Years { get; set; } = Enumerable.Range(DateTime.Now.Year - 100, 100 - 18 + 1).Reverse().ToList();
}
