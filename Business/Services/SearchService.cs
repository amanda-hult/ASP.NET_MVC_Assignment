using Business.Interfaces;
using Business.Models;
using Data.Interfaces;

namespace Business.Services;

public class SearchService(IProjectRepository projectRepository, IUserRepository userRepository, IClientRepository clientRepository) : ISearchService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<IEnumerable<SearchResultModel>> GetSearchResultsAsync(string term)
    {
        var projects = await _projectRepository.GetProjectsByStringAsync(term);
        var users = await _userRepository.GetUsersByStringAsync(term);
        var clients = await _clientRepository.GetClientsByStringAsync(term);

        var searchResult = new List<SearchResultModel>();

        foreach (var project in projects)
        {
            searchResult.Add(new SearchResultModel
            {
                SearchResultImage = project.ProjectImageUrl,
                Title = project.ProjectName,
                Type = "Projects",
                Url = "/projects",
                Id = project.ProjectId,
            });
        }

        foreach (var user in users)
        {
            searchResult.Add(new SearchResultModel
            {
                SearchResultImage = user.UserImageUrl,
                Title = user.FirstName + " " + user.LastName,
                Type = "Members",
                Url = "/members",
            });
        }

        foreach (var client in clients)
        {
            searchResult.Add(new SearchResultModel
            {
                SearchResultImage = client.ClientImageUrl,
                Title = client.ClientName,
                Type = "Clients",
                Url = "/clients",
                Id = client.ClientId,
            });
        }
        return searchResult;
    }
}
