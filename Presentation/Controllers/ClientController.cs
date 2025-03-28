using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;

public class ClientController : Controller
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

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
        if (result != 201)
        {
            return StatusCode(500);
        }

        return RedirectToAction("Clients", "Admin");
    }



    [HttpPost]
    public IActionResult EditClient(EditClientModel model)
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

        //send data to clientservice
        return RedirectToAction("Clients", "Admin");
    }
}
