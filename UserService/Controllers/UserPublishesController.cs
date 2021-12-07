using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserService.Database;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPublishesController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly DatabaseContext _context;
        public UserPublishesController(IBus bus, DatabaseContext context)
        {
            _context = context;
            _bus = bus;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUserTask(TaskMsg taskMsg)
        {
            string claimsEmail;

            if (taskMsg != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    claimsEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    Console.WriteLine("Valid Token");
                }
                else
                {
                    Console.WriteLine("InValid Token");
                    return Forbid();
                }

                //var handler = new JwtSecurityTokenHandler();
                //var decodedValue = handler.ReadJwtToken(taskMsg.Token);

                //var email = decodedValue.Claims.First(claim => claim.Type == "sub").Value;

                var userData = await _context.UsersDb
                    .Where(x => x.Email == claimsEmail)
                    .FirstOrDefaultAsync();

                taskMsg.EmpId = userData.EmpId;
                taskMsg.StartTime = DateTime.Now;
                Uri uri = new Uri("rabbitmq://localhost/theUserTaskQueue");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(taskMsg);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateUserTask(TaskMsg taskMsg)
        {
            string claimsEmail;

            if (taskMsg != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    claimsEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    Console.WriteLine("Valid Token");
                }
                else
                {
                    Console.WriteLine("InValid Token");
                    return Forbid();
                }

                //var handler = new JwtSecurityTokenHandler();
                //var decodedValue = handler.ReadJwtToken(taskMsg.Token);

                //var email = decodedValue.Claims.First(claim => claim.Type == "sub").Value;

                var userData = await _context.UsersDb
                    .Where(x => x.Email == claimsEmail)
                    .FirstOrDefaultAsync();




                taskMsg.EmpId = userData.EmpId;
                taskMsg.StartTime = DateTime.Now;

                Uri uri = new Uri("rabbitmq://localhost/theUserTaskQueue");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(taskMsg);
                return Ok();
            }
            return BadRequest();
        }
    }
}
