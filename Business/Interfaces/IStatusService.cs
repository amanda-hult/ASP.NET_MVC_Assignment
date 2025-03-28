using Business.Models;
using Data.Entities;

namespace Business.Interfaces;

public interface IStatusService
{
    Task<IEnumerable<StatusModel>> GetAllStatuses();
    Task<StatusModel> GetStatusAsync(int? id);
    Task<StatusEntity> GetStatusEntityAsync(int id);
}
