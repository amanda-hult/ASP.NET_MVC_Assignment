using Business.Interfaces;
using Business.Models.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Handlers;
using Presentation.Models;
using Presentation.Services;

namespace Presentation.Controllers;

[Authorize]
public class ClientController(IClientService clientService, IFileHandler fileHandler, HelperService helperService) : Controller
{
    private readonly IClientService _clientService = clientService;
    private readonly IFileHandler _fileHandler = fileHandler;
    private readonly HelperService _helperService = helperService;

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

        var (succeeded, statuscode, clientId) = await _clientService.CreateClientAsync(clientCreateModel);
        if (succeeded)
            await _helperService.HandleNotifications(clientId.ToString(), 3, imageUri, "Add", false);

        if (statuscode == 409)
            return Conflict(new { message = "Client with the same name already exists." });

        if (statuscode == 500)
            return StatusCode(500);

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
            if (!string.IsNullOrEmpty(model.ExistingImage))
            {
                imageUri = model.ExistingImage;
            }
            else
            {
                imageUri = "/images/client-avatar-standard.svg";
            }
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
            await _helperService.HandleNotifications(model.Id.ToString(), 3, imageUri, "Edit", false);
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
            return RedirectToAction("Clients", "Admin");

        if (result == 409)
            return Conflict(new { message = "Client cannot be deleted, since it exists in a project."});

        if (result == 404)
            return NotFound(new { message = "Client not found." });

        return StatusCode(500, new { message = "Unexpected error." });
    }
    #endregion
}
