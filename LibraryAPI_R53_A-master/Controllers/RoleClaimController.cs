using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI_R53_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleClaimController : ControllerBase
    {
        [HttpGet("admin-role")]
        [Authorize(Roles ="Admin")]
        public IActionResult Get()
        {
            return Ok("Admin role");
        }

        [HttpGet("admin-manager-role")]
        [Authorize(Roles ="Admin, Manager")]
        public IActionResult AdminManager()
        {
            return Ok("Admin manager role"); 
        }
    }
}
