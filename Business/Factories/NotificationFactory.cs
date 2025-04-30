using Business.Models.Notifications;
using Data.Entities;

namespace Business.Factories;

public static class NotificationFactory
{
    public static NotificationEntity Create(NotificationCreateModel model)
    {
        return new NotificationEntity
        {
            NotificationTypeId = model.NotificationTypeId,
            TargetGroupId = model.TargetGroupId,
            Image = model.Image,
            Message = model.Message,
        };
    }

    public static NotificationModel Create(NotificationEntity entity)
    {
        return new NotificationModel
        {
            NotificationId = entity.NotificationId,
            TargetGroupId = entity.TargetGroupId,
            NotificationTypeId = entity.NotificationTypeId,
            Image = entity.Image,
            Message = entity.Message,
            Created = entity.Created,
        };
    }
}
