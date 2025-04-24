using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.API.Interface;
using WebBlog.API.Models;
using WebBlog.API.ViewModel.Dto;

namespace WebBlog.API.Controllers
{
    [ApiController]
    //[Authorize(Roles = "User")]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {   
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _token;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _token = tokenService;
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
                await _signInManager.SignInAsync(user, false);

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
            if(user == null)
            {
                return Unauthorized("Invalid username or password");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password,true);
            if(!result.Succeeded)
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
    }
}
