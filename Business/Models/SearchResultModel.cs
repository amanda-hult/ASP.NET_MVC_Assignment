namespace Business.Models;

public class SearchResultModel
{
    public string? SearchResultImage { get; set; }
    public string Title { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Url { get; set; } = null!;
    public int? Id { get; set; }
}
