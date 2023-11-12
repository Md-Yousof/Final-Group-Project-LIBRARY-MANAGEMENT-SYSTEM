using LibraryAPI_R53_A.Core;
using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.DTOs.Account;
using LibraryAPI_R53_A.Persistence.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryAPI_R53_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    #region Step 10
    public class AccountController : ControllerBase
    {
        private readonly JWTService _jwtService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(JWTService jWTService, SignInManager<ApplicationUser>  signInManager, UserManager<ApplicationUser> userManager)
        {
            _jwtService = jWTService;
            _signInManager = signInManager; 
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet("refersh-user-token")]
        public async Task<ActionResult<UserDto>> RefershToken()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            return await CreateApplicationUserDto(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if(user == null)
            {
                return Unauthorized("Invalid username or password");

            }
            if(user.EmailConfirmed == false) {
                return Unauthorized("please confirm your email.");
            }
            // CheckPasswordSignInAsync = true. then unauthenticate person try to lgin. he's account will be locked. 
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.IsLockedOut)
            {
                return Unauthorized(string.Format("Your Account is locked.", user.LockoutEnd));
            }

            if(!result.Succeeded)
            {
                return Unauthorized("Invalid userName or password");
            }

            return await CreateApplicationUserDto(user);
        }

        [HttpPost("register")]  // /api/account/register
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if(await CheckEmailExistsAsync(model.Email))
            {
                return BadRequest($"An existing account is using {model.Email}, email address. please try with anothher email address");
            }

            var userToAdd = new ApplicationUser
            {
                UserName = model.UserName.ToLower(),
                Email = model.Email.ToLower(),
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(userToAdd, model.Password);
            if(!result.Succeeded) return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(userToAdd, SD.UserRole);

            return Ok("Your account has been created, try to login");
        }


        #region Private Helper methods
        private async Task<UserDto> CreateApplicationUserDto(ApplicationUser user)
        {
            return new UserDto
            {
                UserName = user.UserName,
                JWT =await _jwtService.CreateJWT(user)
            };
        }

        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.Users.AnyAsync(x =>x.Email == email.ToLower());
        }
        #endregion
    }
    #endregion
}
