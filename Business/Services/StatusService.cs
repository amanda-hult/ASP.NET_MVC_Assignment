using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class StatusService(IStatusRepository statusRepository) : IStatusService
{
    private readonly IStatusRepository _statusRepository = statusRepository;

    #region Read
    public async Task<IEnumerable<StatusModel>> GetAllStatuses()
    {
        var list = await _statusRepository.GetAllAsync();
        var statusList = list.Select(StatusFactory.Create).ToList();
        return statusList;
    }

    public async Task<StatusModel> GetStatusAsync(int? id)
    {
        var statusEntity = await _statusRepository.GetAsync(x => x.StatusId == id);
        if (statusEntity == null)
            return null;

        var status = StatusFactory.Create(statusEntity);

        return status;
    }

    public async Task<StatusEntity> GetStatusEntityAsync(int? id)
    {
        var statusEntity = await _statusRepository.GetAsync(x => x.StatusId == id);
        if (statusEntity == null)
            return null;

        return statusEntity;
    }
    #endregion
}
