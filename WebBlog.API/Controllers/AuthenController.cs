using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using WebBlog.API.Interface;
using WebBlog.API.Models;
using WebBlog.API.ViewModel.Dto;

namespace WebBlog.API.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AuthenController : ControllerBase
    {   
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _token;
        private readonly IEmailService _emailSender;
        public AuthenController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService,IEmailService emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _token = tokenService;
            _emailSender = emailSender;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                var roleExists = await _roleManager.RoleExistsAsync("User");
                if(!roleExists)
                {
                    var role = new IdentityRole("User");
                    await _roleManager.CreateAsync(role);
                }
                await _userManager.AddToRoleAsync(user, "User");
                return Ok("User created successfully");
            }
            else
            {
                return StatusCode(500, result.Errors);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("Invalid username or password");
            }
            return Ok(new UserDto
            {
                UserName = user.UserName!,
                Email = user.Email!,
                Token = await _token.CreateToken(user)
            });
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByEmailAsync(forgot.Email);
            if(user == null)
            {
                return NotFound(user);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string>
            {
                { "token",token },
                {"email",forgot.Email }

            };
            var callback = QueryHelpers.AddQueryString(forgot.ClientUri!, param);
            var message = new Message([user.Email], "Reset password token", callback);
            _emailSender.SendEmail(message);
            return Ok();
        }
    }
}
