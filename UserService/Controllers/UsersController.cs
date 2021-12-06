using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectForAssignment.Manager;
using UserService.Contracts;
using UserService.Database;
using UserService.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILoggerService _logger;

        private readonly DatabaseContext _context;

        private readonly IConfiguration Configuration;

        public UsersController(DatabaseContext context, ILoggerService logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            Configuration = configuration;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersDb()
        {
            string claimsEmail;

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
            //Show except password
            _logger.LogInfo("User data has been retrieved");
            var users = await _context.UsersDb.ToListAsync();
            //return users.Select(u => u.Email).ToList();
            return users.Select(u => new UserDTO() {
                EmpId = u.EmpId,
                UserName = u.UserName,
                Email = u.Email,
                Designation = u.Designation
            }).ToList();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
           
            var user = await _context.UsersDb.FindAsync(id);

            if (user == null) { return NotFound(); }

            string claimsEmail;

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

            if (claimsEmail != null && claimsEmail.Equals(user.Email))
            {
                //string msg = "User data with id" + id + "has been retrieved";
                _logger.LogInfo("User data with id" + id.ToString() + "has been retrieved");
                 
                System.Diagnostics.Debug.WriteLine("User data with id " + id.ToString() + " has been retrieved");
            }
            else
            {
                return Forbid();
            }

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {

            string claimsEmail;

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

            if (claimsEmail != null && claimsEmail.Equals(user.Email))
            {

                string msg = "User data with id" + id + "has been updated";
                _logger.LogInfo(msg);

                _context.Entry(user).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id)) { 
                        return NotFound(); 
                    }
                    else { 
                        throw; 
                    }
                }

                return Ok();

            }
            else { return Forbid(); }
        }

        // POST: api/Users
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> PostUser(User user)
        {
            //int? x = 5;
            //var y = x ?? 1;

            //check if the email matches the reg-ex
            var valid = EmailManager.EmailValidation(user.Email);
            if (!valid)
            {
                return BadRequest();
            }

            //Hash the user.password
            var hashedPassword = PasswordManager.HashPassword(user.Password);

            string msg = "New data has been created";
            _logger.LogInfo(msg);

            user.Password = hashedPassword;

            _context.UsersDb.Add(user);
            await _context.SaveChangesAsync();

            string token = ValidateUser(user.Email);

            UserDTO userDto = user.Adapt<UserDTO>();
            userDto.Token = token;

            return CreatedAtAction("GetUser", new { id = user.EmpId }, userDto);
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



            string claimsEmail;
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

            if (claimsEmail != null && claimsEmail.Equals(user.Email))
            {
                string msg = "User data with id" + id + "has been deleted";
                _logger.LogWarn(msg);

                _context.UsersDb.Remove(user);
                await _context.SaveChangesAsync();

                return user;

            }
            else
            {
                return Forbid();
            }

            
        }

        [HttpPost("generate")]
        [AllowAnonymous]
        public ActionResult<string> GenerateToken(UserDTO user)
        {
            //password
            var token = ValidateUser(user.Email);

            return token;
        }

        private bool UserExists(int id)
        {
            return _context.UsersDb.Any(e => e.EmpId == id);
        }

        private string ValidateUser(string email){

            string key = Configuration["SecretKey"]; //Secret key   
            string issuer = Configuration["Issuer"];

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> permClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, email)
            };

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);

            Console.WriteLine(jwt_token);

            return jwt_token;
        }

        public static string ValidateToken(string token)
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null) return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;
            return username;
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Convert.FromBase64String("ERMN05OPLoDvbTTa/QkqLNMI7cPLguaRyHzyg7n5qNBVjQmtBhz4SzYh4NBVCXi3KJHlSXKP+oi2+bXr6CUYTR==");
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
 