using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AquaWater.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationRepo;
        public NotificationController(INotificationService notificationRepo)
        {
            _notificationRepo = notificationRepo;
        }

        [HttpPost]
        public async Task<ActionResult> MarkNotification(List<string> notification)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                await _notificationRepo.MarkNotificationAsRead(notification, userId);
                return Ok();
            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return StatusCode(500);
            }
        }
    }
}
