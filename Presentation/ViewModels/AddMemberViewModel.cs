using Presentation.Models;

namespace Presentation.ViewModels;

public class AddMemberViewModel
{
    public string Title { get; set; } = "Add Member";
    public AddMemberModel AddMemberModel { get; set; } = new AddMemberModel();

    public IEnumerable<int> Days { get; set; } = Enumerable.Range(1, 31).ToList();
    public IEnumerable<string> Months { get; set; } = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
    public IEnumerable<int> Years { get; set; } = Enumerable.Range(DateTime.Now.Year - 100, 100 - 18 + 1).Reverse().ToList();
}
