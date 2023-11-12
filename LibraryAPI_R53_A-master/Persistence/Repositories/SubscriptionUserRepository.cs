using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Entities;
using LibraryAPI_R53_A.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI_R53_A.Persistence.Repositories
{
    public class SubscriptionUserRepository : ISubscriptionUser
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRequest(SubscriptionUser subscriptionUser)
        {
            _context.SubscriptionUsers.Add(subscriptionUser);
            await _context.SaveChangesAsync();
        }

        public async Task<SubscriptionUser?> GetPendingRequest(string userId)
        {
            return await _context.SubscriptionUsers
                .FirstOrDefaultAsync(su => su.ApplicationUserId == userId && su.Accepted == null);
        }

        public async Task<IEnumerable<SubscriptionUser>> GetAll()
        {
            return await _context.SubscriptionUsers.ToListAsync();

        }
        public async Task<SubscriptionUser?> GetById(int subscriptionUserId)
        {
            return await _context.SubscriptionUsers.FirstOrDefaultAsync(su => su.SubscriptonUserId == subscriptionUserId);
        }

        public async Task Update(SubscriptionUser subscriptionUser)
        {
            _context.Entry(subscriptionUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

    }
}
