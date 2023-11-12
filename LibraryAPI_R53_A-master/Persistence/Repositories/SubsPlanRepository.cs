using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI_R53_A.Persistence.Repositories
{
    public class SubsPlanRepository : ISubscriptionPlan
    {
        private readonly ApplicationDbContext _context;


        public SubsPlanRepository(ApplicationDbContext context)
        {
            _context = context;

        }


        public async Task<IEnumerable<SubscriptionPlan>> GetAll()
        {
            return await _context.SubscriptionPlans.ToListAsync();
        }

        public async Task<SubscriptionPlan?> Get(int id)
        {
            var SubscriptionPlan = await _context.SubscriptionPlans.FindAsync(id);
            return SubscriptionPlan;
        }

        public async Task<SubscriptionPlan?> Post(SubscriptionPlan entity)
        {
            if (_context.SubscriptionPlans.Any(p => p.PlanName == entity.PlanName))
            {
                return null;
            }
            _context.SubscriptionPlans.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Put(SubscriptionPlan SubscriptionPlan)
        {
            _context.SubscriptionPlans.Update(SubscriptionPlan);
            await _context.SaveChangesAsync();

        }

        public async Task Delete(int id)
        {
            var SubscriptionPlan = await _context.SubscriptionPlans.FindAsync(id);
            if (SubscriptionPlan != null)
            {
                _context.SubscriptionPlans.Remove(SubscriptionPlan);
                await _context.SaveChangesAsync();
            }
        }

        public IEnumerable<SubscriptionPlan> Search(string searchString)
        {
            var subscriptionPlan = from s in _context.SubscriptionPlans
                            where s.PlanName.ToLower().Contains(searchString.ToLower())
                            select s;
            return subscriptionPlan.ToList();
        }
        public IEnumerable<SubscriptionPlan> GetActive()
        {
            var subscriptionPlan = from s in _context.SubscriptionPlans
                            where s.IsActive == true
                            select s;
            return subscriptionPlan;
        }

        public IEnumerable<SubscriptionPlan> GetInactive()
        {
            var subscriptionPlan = from s in _context.SubscriptionPlans
                            where s.IsActive == false
                            select s;
            return subscriptionPlan;
        }
    }
}
