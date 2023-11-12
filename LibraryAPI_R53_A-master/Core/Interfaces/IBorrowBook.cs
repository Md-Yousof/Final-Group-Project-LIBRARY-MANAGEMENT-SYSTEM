using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI_R53_A.Core.Interfaces
{
    public interface IBorrowBook : IRepository<BorrowedBook>
    {
        Task<IEnumerable<BorrowedBook>> GetAllRequestedBooksByUserName(string userName);
        Task<IEnumerable<BorrowedBook>> GetAllCancelledBooks();
        Task<IEnumerable<BorrowedBook>> GetAllApprovedBooks();
        Task<BorrowedBook> ApproveBorrowedBookAsync(BorrowedBook borrowedBook);
        Task<BorrowedBook> CancelBorrowedBookAsync(BorrowedBook borrowedBook);
        Task<IEnumerable<BorrowedBook>> GetAllByUserName(string userName);
        Task<IEnumerable<BorrowedBook>> GetAllRequestedBooksByUserId(string userId);
        Task<BorrowedBook> ReturnBook(BorrowedBook borrowedBook, string remarks,decimal miscFine);

        Task<IActionResult> SendBookRequest(string userId, BorrowedBook borrowedBook);


    }
}
