using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Entities;
using LibraryAPI_R53_A.Core.Interfaces;
using LibraryAPI_R53_A.Persistence.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace LibraryAPI_R53_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionUserController : ControllerBase
    {
        private readonly ISubscriptionUser _subscriptionUserRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        public SubscriptionUserController(ISubscriptionUser subscriptionUserRepository, UserManager<ApplicationUser> user )
        {
            _subscriptionUserRepository = subscriptionUserRepository;
            _userManager = user;
        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var s = await _subscriptionUserRepository.GetAll();
            return Ok(s);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendSubscriptionRequest(int subscriptionPlanId, string transactionId)
        {
            try
            {
                var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated.");
                }

                // Check if the user already has a pending request
                var existingRequest = await _subscriptionUserRepository.GetPendingRequest(userId);

                if (existingRequest != null)
                {
                    return BadRequest("A pending request already exists.");
                }

                var subscriptionUser = new SubscriptionUser
                {
                    ApplicationUserId = userId,
                    SubscriptionPlanId = subscriptionPlanId,
                    TransactionId = transactionId,
                    Accepted = null // Not accepted yet
                };

                await _subscriptionUserRepository.AddRequest(subscriptionUser);
                return Ok("Request sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("accept/{subscriptionUserId}")]
        public async Task<IActionResult> AcceptSubscriptionRequest(int subscriptionUserId)
        {
            try
            {
                var subscriptionUser = await _subscriptionUserRepository.GetById(subscriptionUserId);

                if (subscriptionUser == null)
                {
                    return NotFound("Subscription user request not found.");
                }
                subscriptionUser.Accepted = true;

                var user = await _userManager.FindByIdAsync(subscriptionUser.ApplicationUserId);

                if (user != null)
                {
                    user.IsSubscribed = true; // Set the IsSubscribed property to true
                    await _userManager.UpdateAsync(user);
                }

                await _subscriptionUserRepository.Update(subscriptionUser);

                return Ok("Request accepted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("reject/{subscriptionUserId}")]
        public async Task<IActionResult> RejectSubscriptionRequest(int subscriptionUserId)
        {
            try
            {
                var subscriptionUser = await _subscriptionUserRepository.GetById(subscriptionUserId);

                if (subscriptionUser == null)
                {
                    return NotFound("Subscription user request not found.");
                }

                subscriptionUser.Accepted = false;
                await _subscriptionUserRepository.Update(subscriptionUser);

                return Ok("Request rejected.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}
