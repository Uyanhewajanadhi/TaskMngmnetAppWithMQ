using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectForAssignment.Manager;
using UserService.Contracts;
using UserService.Database;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILoggerService _logger;

        private readonly DatabaseContext _context;

        public UsersController(DatabaseContext context, ILoggerService logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersDb()
        {
            //Show except password
            _logger.LogInfo("User data has been retrieved");
            var users = await _context.UsersDb.ToListAsync();
            //return users.Select(u => u.Email).ToList();
            return users.Select(u => new User() {

                UserName = u.UserName,
                Email = u.Email,
                Designation = u.Designation
            }).ToList();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            //Return details except password
            string msg = "User data with id" + id + "has been retrieved";
            _logger.LogInfo(msg);
            var user = await _context.UsersDb.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            //First check wether previous password matches

            //Hash the user.password if it is getting input

            //check if the email matches the reg-ex 

            if (id != user.EmpId)
            {
                return BadRequest();
            }

            string msg = "User data with id" + id + "has been updated";
            _logger.LogInfo(msg);

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            //Hash the user.password
            var hashedPassword = PasswordManager.HashPassword(user.Password);

            //check if the emial mathces th ereg-ex
            var valid = EmailManager.EmailValidation(user.Email);
            if (!valid)
            {
                return BadRequest();
            }

            string msg = "New data has been created";
            _logger.LogInfo(msg);

            user.Password = hashedPassword;

            _context.UsersDb.Add(user);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetUser", new { id = user.EmpId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.UsersDb.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            string msg = "User data with id" + id + "has been deleted";
            _logger.LogWarn(msg);

            //get the password checked before deleting the data.
            //so we need to check wther this password  matches with hashed password in the DB

            _context.UsersDb.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.UsersDb.Any(e => e.EmpId == id);
        }

        //validateUser(){
        //  
        //}
    }
}
 