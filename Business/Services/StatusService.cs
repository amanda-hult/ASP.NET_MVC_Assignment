using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;

namespace Business.Services;

public class StatusService(IStatusRepository statusRepository) : IStatusService
{
    private readonly IStatusRepository _statusRepository = statusRepository;

    public async Task<IEnumerable<StatusModel>> GetAllStatuses()
    {
        var list = await _statusRepository.GetAllAsync();
        var statusList = list.Select(StatusFactory.Create).ToList();
        return statusList;
    }
}
