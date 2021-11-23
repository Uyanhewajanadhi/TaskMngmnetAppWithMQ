using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPublishesController : ControllerBase
    {
        private readonly IBus _bus;
        public UserPublishesController(IBus bus)
        {
            _bus = bus;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUserTask(TaskMsg taskMsg)
        {
            if (taskMsg != null)
            {
                taskMsg.startTime = DateTime.Now;
                Uri uri = new Uri("rabbitmq://localhost/userTaskQueue");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(taskMsg);
                return Ok();
            }
            return BadRequest();
        }
    }
}
