using System.Security.Claims;
using Business.Interfaces;
using Business.Models.Clients;
using Business.Models.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.Handlers;
using Presentation.Hubs;
using Presentation.Models;

namespace Presentation.Controllers;

public class ClientController : Controller
{
    private readonly IClientService _clientService;
    private readonly INotificationService _notificationService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly IFileHandler _fileHandler;

    public ClientController(IClientService clientService, INotificationService notificationService, IHubContext<NotificationHub> hubContext, IFileHandler fileHandler)
    {
        _clientService = clientService;
        _notificationService = notificationService;
        _hubContext = hubContext;
        _fileHandler = fileHandler;
    }

    #region Add Client
    [HttpPost]
    public async Task<IActionResult> AddClient(AddClientModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                );

            return BadRequest(new { success = false, errors });
        }

        string imageUri;

        if (model.ClientImage == null || model.ClientImage.Length == 0)
        {
            imageUri = "/images/client-avatar-standard.svg";
        }
        else
        {
            imageUri = await _fileHandler.UploadFileAsync(model.ClientImage);
        }

        var clientCreateModel = new ClientCreateModel
        {
            ClientImage = imageUri,
            ClientName = model.ClientName,
            Email = model.Email,
            Location = model.Location,
            Phone = model.Phone,
        };

        var result = await _clientService.CreateClientAsync(clientCreateModel);
        if (result == 201)
        {
            var client = await _clientService.GetClientByNameAsync(model.ClientName);

            if (client != null)
            {
                var notificationCreateModel = new NotificationCreateModel
                {
                    Message = $"{client.ClientName} was added.",
                    NotificationTypeId = 3
                };

                await _notificationService.AddNotificationAsync(notificationCreateModel);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var notifications = await _notificationService.GetNotificationsAsync(userId);
                var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();
                if (newNotification != null)
                {
                    await _hubContext.Clients.All.SendAsync("AdminRecieveNotification", newNotification);
                }
            }
        }

        if (result == 409)
        {
            return Conflict(new { message = "Client with the same name already exists." });
        }

        return RedirectToAction("Clients", "Admin");
    }
    #endregion

    #region Edit Client
    [HttpPost]
    public async Task<IActionResult> EditClient(EditClientModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                );

            return BadRequest(new { success = false, errors });
        }

        string imageUri;

        if (model.ClientImage == null || model.ClientImage.Length == 0)
        {
            imageUri = "/images/client-avatar-standard.svg";
        }
        else
        {
            imageUri = await _fileHandler.UploadFileAsync(model.ClientImage);
        }

        var clientEditModel = new ClientEditModel
        {
            Id = model.Id,
            ClientImage = imageUri,
            ClientName = model.ClientName,
            Email = model.Email,
            Location = model.Location,
            Phone = model.Phone,
        };

        var result = await _clientService.UpdateClientAsync(clientEditModel);

        if (result == 200)
        {
            var client = await _clientService.GetClientAsync(model.Id);
            if (client != null)
            {
                var notificationCreateModel = new NotificationCreateModel
                {
                    Message = $"Client {client.ClientName} was updated.",
                    NotificationTypeId = 3,
                    Image = client.ClientImage
                };

                await _notificationService.AddNotificationAsync(notificationCreateModel);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var notifications = await _notificationService.GetNotificationsAsync(userId);
                var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();
                if (newNotification != null)
                {
                    await _hubContext.Clients.All.SendAsync("AdminRecieveNotification", newNotification);
                }
            }

            return RedirectToAction("Clients", "Admin");
        }

        if (result == 409)
            return Conflict(new { message = "Client with the same name already exists." });

        if (result == 404)
            return Conflict(new { message = "Client not found." });

        if (result == 500)
            return StatusCode(500, new { message = "Unexpected error." });

        return View(model);
    }
    #endregion

    #region Delete Client
    [HttpPost]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var result = await _clientService.DeleteClientAsync(id);

        if (result == 204)
        {

            var client = await _clientService.GetClientAsync(id);
            if (client != null)
            {
                var notificationCreateModel = new NotificationCreateModel
                {
                    Message = $"Client {client.ClientName} was deleted.",
                    NotificationTypeId = 3
                };

                await _notificationService.AddNotificationAsync(notificationCreateModel);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var notifications = await _notificationService.GetNotificationsAsync(userId);
                var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();
                if (newNotification != null)
                {
                    await _hubContext.Clients.All.SendAsync("RecieveNotification", newNotification);
                }
            }

            return RedirectToAction("Clients", "Admin");
        }

        if (result == 409)
            return Conflict(new { message = "Client cannot be deleted, since it exists in a project."});

        if (result == 404)
            return NotFound(new { message = "Client not found." });

        return StatusCode(500, new { message = "Unexpected error." });
    }
    #endregion
}
