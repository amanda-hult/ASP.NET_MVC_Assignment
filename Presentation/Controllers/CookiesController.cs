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

        if (consent.Functional)
        {
            Response.Cookies.Append("DarkMode", "Non-Essential", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(120),
                SameSite = SameSiteMode.Lax,
                Path = "/",
                Secure = true
            });
        }
        else
        {
            Response.Cookies.Delete("DarkMode");
        }

        if (consent.Analytics)
        {
            Response.Cookies.Append("Analytics", "Non-Essential", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(120),
                SameSite = SameSiteMode.Lax,
                Path = "/",
                Secure = true
            });
        }
        else
        {
            Response.Cookies.Delete("Analytics");
        }

        if (consent.Marketing)
        {
            Response.Cookies.Append("Marketing", "Non-Essential", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(120),
                SameSite = SameSiteMode.Lax,
                Path = "/",
                Secure = true
            });
        }
        else
        {
            Response.Cookies.Delete("Marketing");
        }

        return Ok();
    }
}
