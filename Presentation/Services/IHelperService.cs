namespace Presentation.Services;

public interface IHelperService
{
    Task HandleNotifications(string? id, int notificationTypeId, string imageUri, string action, bool showAll);
}