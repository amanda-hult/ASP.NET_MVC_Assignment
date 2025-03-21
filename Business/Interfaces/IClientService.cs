using Business.Models;

namespace Business.Interfaces;

public interface IClientService
{
    Task<int> CreateClient(ClientCreateModel model);
}
