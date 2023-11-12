using AutoMapper;
using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Interfaces;
using LibraryAPI_R53_A.Core.Repositories;
using LibraryAPI_R53_A.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI_R53_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPlansController : ControllerBase
    {
        private ISubscriptionPlan _subscriptionPlan;
        private readonly IMapper _mapper;
        public SubscriptionPlansController(ISubscriptionPlan subscriptionPlan, IMapper map)
        {
            _subscriptionPlan = subscriptionPlan;
            _mapper = map;
        }

        [HttpGet]
        public async Task<IActionResult> GetsubscriptionPlans()
        {
            var subscriptionPlan = await _subscriptionPlan.GetAll();
            return Ok(subscriptionPlan);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var subscriptionPlan = await _subscriptionPlan.Get(id);
            return Ok(subscriptionPlan);
        }

        [HttpPost]
        public async Task<IActionResult> Post(SubscriptionPlan model)
        {
            await _subscriptionPlan.Post(model);
            return Ok(model);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> Edit(SubscriptionPlanDto subscriptionPlanDt)
        {
            var updated = _mapper.Map<SubscriptionPlan>(subscriptionPlanDt);

            await _subscriptionPlan.Put(updated);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            await _subscriptionPlan.Delete(id);
            return Ok(new { message = "Deleted successfully" });

        }



        [HttpGet, Route("SearchSubscription/{searchString}")]
        public IActionResult SearchBySubscriptionName(string searchString)
        {
            var subscriptionPlan = _subscriptionPlan.Search(searchString);
            return Ok(subscriptionPlan);
        }

        [HttpGet, Route("GetActiveSubscription")]
        public IActionResult GetActiveSubscription()
        {
            var subscriptionPlan = _subscriptionPlan.GetActive();
            return Ok(subscriptionPlan);
        }

        [HttpGet, Route("GetInactiveSubscriptions")]
        public IActionResult GetInactiveSubscription()
        {
            var subscriptionPlan = _subscriptionPlan.GetInactive();
            return Ok(subscriptionPlan);
        }



    }
}
