using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Repositories;

namespace LibraryAPI_R53_A.Core.Interfaces
{
    public interface IBookCopy:IRepository<BookCopy>
    {
        IEnumerable<BookCopy> GetDamaged();
        IEnumerable<BookCopy> GotToRepair();
        IEnumerable<BookCopy> GoodCondition();
        Task<BookCopy> GetAvailable(int? bookId);
        Task ChangeAvailability(int id, bool isAvailable);
    }
}
