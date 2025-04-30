using Business.Models;

namespace Business.Interfaces;

public interface ISearchService
{
    Task<IEnumerable<SearchResultModel>> GetSearchResultsAsync(string term);
}