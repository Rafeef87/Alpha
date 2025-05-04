using Business.Services;
using Domain.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationControllerr(INotificationService notificationService) : Controller
{
    private readonly INotificationService _notificationService = notificationService;

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) 
            return Unauthorized("User ID is missing.");

        var notifications = await _notificationService.GetUserNotificationsAsync(userId);

        ViewBag.Notifications = notifications;
        return View();
    }

    // GET: api/Notification
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) // Ensure userId is not null or empty
            return Unauthorized("User ID is missing.");

        var notifications = await _notificationService.GetUserNotificationsAsync(userId);
        return Ok(notifications);
    }

    // POST: api/Notification
    [HttpPost]
    public async Task<IActionResult> Send([FromBody] Notification notification)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) // Ensure userId is not null or empty
            return Unauthorized("User ID is missing.");

        try
        {
            await _notificationService.SendNotificationAsync(userId, notification);
            return Ok(new { message = "Notification sent successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error sending notification: {ex.Message}");
        }
    }

    // POST: api/Notification/dismiss/5
    [HttpPost("dismiss/{id}")]
    public async Task<IActionResult> Dismiss(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) // Ensure userId is not null or empty
            return Unauthorized("User ID is missing.");

        try
        {
            await _notificationService.DismissNotificationAsync(userId, id);
            return Ok(new { message = "Notification dismissed." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error dismissing notification: {ex.Message}");
        }
    }
}
