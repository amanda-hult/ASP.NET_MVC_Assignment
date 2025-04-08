using System.Diagnostics;
using System.Security.Claims;
using Business.Interfaces;
using Business.Models.Clients;
using Business.Models.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.Hubs;
using Presentation.Models;

namespace Presentation.Controllers;

public class ClientController : Controller
{
    private readonly IClientService _clientService;
    private readonly INotificationService _notificationService;
    private readonly IHubContext<NotificationHub> _hubContext;

    public ClientController(IClientService clientService, INotificationService notificationService, IHubContext<NotificationHub> hubContext)
    {
        _clientService = clientService;
        _notificationService = notificationService;
        _hubContext = hubContext;
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

        var clientCreateModel = new ClientCreateModel
        {
            ClientImage = model.ClientImage,
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

        var clientCreateModel = new ClientEditModel
        {
            Id = model.Id,
            ClientImage = model.ClientImage,
            ClientName = model.ClientName,
            Email = model.Email,
            Location = model.Location,
            Phone = model.Phone,
        };

        var result = await _clientService.UpdateClientAsync(clientCreateModel);
        Debug.WriteLine($"{result}");

        if (result == 200)
            return RedirectToAction("Clients", "Admin");

        // fix errormessages
        ViewBag.ErrorMessage = "Could not update the client.";
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
