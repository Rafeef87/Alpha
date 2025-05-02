using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class CookiesController : Controller
{
    [HttpPost]
    public IActionResult SetCookies([FromBody] CookieConsent consent)
    {
        if (consent == null)
            return BadRequest("Invalid consent data.");

        // Set cookies based on the consent
        if (consent.Functional)
        {
            // Set analytics cookies
            Response.Cookies.Append("FunctionalCookie", "Non-essential", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Lax,
                Path = "/",
            });
        }
        else
        {
            // Remove analytics cookies if they exist
            Response.Cookies.Delete("FunctionalCookie");
        }

        if (consent.Marketing)
        {
            // Set marketing cookies
            Response.Cookies.Append("MarketingCookie", "Non-essential", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Lax,
                Path = "/",
            });
        }
        else
        {
            // Remove analytics cookies if they exist
            Response.Cookies.Delete("MarketingCookie");
        }


        if (consent.Analytics)
        {
            // Set analytics cookies
            Response.Cookies.Append("AnalyticsCookie", "Non-essential", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Lax,
                Path = "/",
            });
        }

        else
        {
            // Remove analytics cookies if they exist
            Response.Cookies.Delete("AnalyticsCookie");
        }

        if (consent.Essential)
        {
            // Set preferences cookies
            Response.Cookies.Append("EssentialCookie", "Non-essential", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Lax,
                Path = "/",
            });
        }
        else
        {
            // Remove analytics cookies if they exist
            Response.Cookies.Delete("EssentialCookie");
        }
        return Ok();
    }
}
