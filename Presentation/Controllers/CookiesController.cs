using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;

public class CookiesController : Controller
{
    [HttpPost]
    public IActionResult SetCookies([FromBody] CookieConsent consent)
    {

        if (consent == null)
            return BadRequest();


        return Ok();
    }
}
