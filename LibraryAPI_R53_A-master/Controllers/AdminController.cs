using LibraryAPI_R53_A.Core;
using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.DTOs.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace LibraryAPI_R53_A.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser>userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("get-members")]
        public async Task<ActionResult<IEnumerable<MemberViewDto>>> GetMembers()
        {
            var members = await _userManager.Users
                .Where(x => x.UserName != SD.AdminUserName)
                .Select(member => new MemberViewDto
                {
                    id = member.Id,
                    UserName = member.UserName,
                    IsLocked = _userManager.IsLockedOutAsync(member).GetAwaiter().GetResult(),
                    Roles = _userManager.GetRolesAsync(member).GetAwaiter().GetResult()
                }).ToListAsync();

            return Ok(members);
        }

        [HttpGet("get-member/{id}")]
        public async Task<ActionResult<MemberAddEditDto>> GetMember (string id)
        {
            var member = await _userManager.Users
                .Where(x => x.UserName != SD.AdminUserName && x.Id == id)
                .Select(m => new MemberAddEditDto
                {
                    Id = m.Id,
                    UserName = m.UserName,
                    Roles = string.Join(",", _userManager.GetRolesAsync(m).GetAwaiter().GetResult())
                }).FirstOrDefaultAsync();

            return Ok(member);
        }

        [HttpPost("add-edit-member")]
        public async Task<IActionResult> AddEditMember(MemberAddEditDto model)
        {
            ApplicationUser user;

            if (string.IsNullOrEmpty(model.Id)) 
            {
                // adding a new member
                if(string.IsNullOrEmpty(model.Password) || model.Password.Length < 6) 
                {
                    ModelState.AddModelError("errors", "password must be at least 6 charachters");
                    return BadRequest(ModelState);
                }

                user = new ApplicationUser
                {
                    UserName = model.UserName.ToLower(),
                    //Email = model.Email.ToLower(),
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded) return BadRequest(result.Errors);
            }
            else
            {
                // editing an existing member

                if (!string.IsNullOrEmpty(model.Password))
                {
                    if(model.Password.Length < 6)
                    {
                        ModelState.AddModelError("erros", "Password must at least be 6 characther");
                        return BadRequest(ModelState);
                    }
                }

                if (IsAdminUserId(model.Id))
                {
                    return BadRequest(SD.SuperAdminChangedNotAllowed);
                }

                user = await _userManager.FindByIdAsync(model.Id);
                if(user == null)
                {
                    return NotFound();
                }
                
                user.UserName = model.UserName.ToLower();

                
                if (!string.IsNullOrEmpty(model.Password))
                {
                    await _userManager.RemovePasswordAsync(user);
                    await _userManager.AddPasswordAsync(user, model.Password);
                }
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            //Removing existing user roles

            await _userManager.RemoveFromRolesAsync(user, userRoles);

            foreach(var role in model.Roles.Split(',').ToArray())
            {
                var roleToAdd = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == role);
                if(roleToAdd != null)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }

            if (string.IsNullOrEmpty(model.Id))
            {
                return Ok(new JsonResult(new { title = "Member Created", message = $"{model.UserName} has been created" }));
            }
            else
            {
                return Ok(new JsonResult(new { title = "member edited", message = $"{model.UserName} has been updated" }));
            }
        }

        [HttpPut("lock-member/{id}")]
        public async Task<IActionResult> LockMember(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null) return NotFound();

            if(IsAdminUserId(id))
            {
                return BadRequest(SD.SuperAdminChangedNotAllowed);
            }

            await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddDays(5));
            return NoContent();
        }

        [HttpPut("unlock-member/{id}")]
        public async Task<IActionResult> UnlockMember(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (IsAdminUserId(id))
            {
                return BadRequest(SD.SuperAdminChangedNotAllowed);
            }

            await _userManager.SetLockoutEndDateAsync(user, null);
            return NoContent();
        }


        [HttpDelete("delete-member/{id}")]
        public async Task<IActionResult> DeleteMember(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (IsAdminUserId(id))
            {
                return BadRequest(SD.SuperAdminChangedNotAllowed);
            }

            await _userManager.DeleteAsync(user);
            return NoContent();
        }

        [HttpGet("get-application-roles")]
        public async Task<ActionResult<string[]>> GetApplicationUser()
        {
            return Ok(await _roleManager.Roles.Select(x => x.Name).ToListAsync());
        }



        private bool IsAdminUserId(string userId)
        {
            return _userManager.FindByIdAsync(userId).GetAwaiter().GetResult().UserName.Equals(SD.AdminUserName);
        }

        
    }
}
