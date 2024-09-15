using Administration.Application.Interfaces;
using Administration.Application.Interfaces.UserInterface;
using Administration.Application.ViewModels;
using Administration.Application.ViewModels.AuthenticationViewModels;
using Administration.Application.ViewModels.UserViewModels;
using Administration.Domain.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Administration.WebApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthenticationController : Controller
    {
        #region Constructor
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="configuration"></param>
        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, IUserService userService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _userService = userService;
        }
        #endregion

        /// <summary>
        /// Login with an existing account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LogInViewModel model)
        {
            //var user = await userManager.FindByNameAsync(model.Username);
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
                var x = _configuration.GetSection("JWT")["SecretKey"];

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(x));

                var token = new JwtSecurityToken(

                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );


                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo.ToString("dd/MM/yyyy HH:mm:ss")
                });
            }
            return Unauthorized();
        }

        /// <summary>
        /// Create a new user and assign them a new role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("Register")]
        [Authorize]
        public async Task<IActionResult> Register([FromBody] CreateUserViewModel model)
        {

            var admin = await _userService.GetCurrentUserDetailsAsync();
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return BadRequest("Një përdorues me këtë emër egziston!");
            ApplicationUser user = new()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Password = model.Password,
                NormalizedUserName = model.Username.ToLower(),
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedById = admin.Id,
                CreatedOnUtc = DateTime.Now,
                UserName = model.Username,
                
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest("Dicka shkoi keq , ju lutem kontrolloni të dhënat e vendosura!");

            var rolUser = Guid.Parse("021E4E36-8C88-48DF-A3E7-BBBAC08A012E").ToString().ToLower();
            //var role = await roleManager.FindByIdAsync(model.RoleId.ToString());
            var role = await roleManager.FindByIdAsync(rolUser);
            if (role != null)
            {
                try
                {
                    await userManager.AddToRoleAsync(user, role.Name.ToLower());
                }
                catch(Exception ex)
                {
                    return BadRequest("Dicka shkoi keq.");
                }
               
            }
            return Ok("Sucess");
        }

        /// <summary>
        /// Get data regarding users
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Users")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers([FromQuery] string firstName, [FromQuery] string lastName, [FromQuery] string email, [FromQuery] string role, [FromQuery] bool? isActive)
        {
            return Json(await _userService.GetUsers(firstName, lastName, email, role, isActive));
        }

        [HttpPost]
        [Route("User/{id}")]
        [Authorize]

        public IActionResult GetUserById([FromRoute] Guid id)
        {
            return Json(
            _userService.GetUserById(id));
        }
        /// <summary>
        /// Get user roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Roles")]
        [Authorize]

        public IActionResult GetAllRoles()
        {

            return Json(_userService.GetRoles());
        }

        /// <summary>
        /// Get current user details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("CurrentUser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var user = await _userService.GetCurrentUserDetailsAsync();
            return Json(user);
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserViewModel user)
        {
            await _userService.UpdateUser(user);
            return Ok();
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromQuery] Guid Id)
        {
            await _userService.DeleteUser(Id);
            return Ok();
        }
        
    }
}
