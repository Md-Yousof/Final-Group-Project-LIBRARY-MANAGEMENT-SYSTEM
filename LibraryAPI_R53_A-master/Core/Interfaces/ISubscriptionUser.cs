using LibraryAPI_R53_A.Core.Entities;

namespace LibraryAPI_R53_A.Core.Interfaces
{
    public interface ISubscriptionUser
    {
        Task AddRequest(SubscriptionUser subscriptionUser);
        Task<SubscriptionUser?> GetPendingRequest(string userId);
        Task<IEnumerable<SubscriptionUser>> GetAll();
        Task Update(SubscriptionUser subscriptionUser);
        Task<SubscriptionUser?> GetById(int subscriptionUserId);
    }
}
