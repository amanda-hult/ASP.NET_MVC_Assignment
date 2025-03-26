using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class StatusFactory
{
    public static StatusModel Create(StatusEntity entity)
    {
        return new StatusModel
        {
            StatusId = entity.StatusId,
            StatusName = entity.StatusName
        };
    }
}
