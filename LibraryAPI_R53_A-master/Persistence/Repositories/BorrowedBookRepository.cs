using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Entities;
using LibraryAPI_R53_A.Core.Interfaces;
using LibraryAPI_R53_A.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI_R53_A.Persistence.Repositories
{
    public class BorrowedBookRepository : IBorrowBook
    {
        private readonly ApplicationDbContext _context;

        public BorrowedBookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BorrowedBook>> GetAllRequestedBooksByUserName(string userName)
        {
            return await _context.BorrowedBooks
                .Include(b => b.Book)
                .Include(b => b.BookCopy)
                .Where(b => b.UserInfo.UserName == userName && b.Status == "Requested")
                .ToListAsync();
        }

        public async Task<IEnumerable<BorrowedBook>> GetAllRequestedBooksByUserId(string userId)
        {
            return await _context.BorrowedBooks
                .Include(b => b.Book)
                .Include(b => b.BookCopy)
                .Where(b => b.UserInfo.Id == userId)
                .ToListAsync();
        }


        public async Task<IEnumerable<BorrowedBook>> GetAllByUserName(string userName)
        {
            return await _context.BorrowedBooks
                .Include(b => b.Book)
                .Include(b => b.BookCopy)
                .Where(b => b.UserInfo.UserName == userName)
                .ToListAsync();
        }
        public async Task<IEnumerable<BorrowedBook>> GetAllApprovedBooks()
        {
            return await _context.BorrowedBooks
                .Include(b => b.Book)
                .Include(b => b.BookCopy)
                .Where(b => b.Status == "Approved")
                .ToListAsync();

        }

        public async Task<IEnumerable<BorrowedBook>> GetAllCancelledBooks()
        {
            return await _context.BorrowedBooks
                .Include(b => b.Book)
                .Include(b => b.BookCopy)
                .Where(b => b.Status == "Cancelled")
                .ToListAsync();
        }

        public async Task<IEnumerable<BorrowedBook>> GetAll()
        {
            return await _context.BorrowedBooks
                .Include(b => b.Book)
                .Include(b => b.BookCopy)
                .ToListAsync();
        }

        public async Task<BorrowedBook> ApproveBorrowedBookAsync(BorrowedBook borrowedBook)
        {
            if (borrowedBook == null)
            {
                throw new InvalidOperationException("BorrowedBook must have a valid value.");
            }

            borrowedBook.BorrowDate = DateTime.Now;
            borrowedBook.DueDate = DateTime.Now.AddDays(7);
            borrowedBook.Status = "Approved";
            borrowedBook.Comment = "";


            var invoice = new Invoice();
            invoice.BorrowedBook = borrowedBook;
            invoice.UserId = borrowedBook.UserId;
            invoice.TransactionDate = DateTime.Now;

            if (borrowedBook.UserInfo?.IsSubscribed == true)
            {
                invoice.Payment = 0;
                invoice.Refund = 0;
            }
            else
            {
                invoice.Payment = (decimal)borrowedBook.Book.BookPrice;
                invoice.Refund = borrowedBook?.Book?.BookPrice * 0.7m;

            }
            _context.Invoices.Add(invoice);

            // Add the invoice to the context and save changes
            await _context.SaveChangesAsync();

            return borrowedBook;
        }


        public async Task<BorrowedBook> CancelBorrowedBookAsync(BorrowedBook borrowedBook)
        {

            borrowedBook.DueDate = null;
            borrowedBook.Status = "Cancelled";

            await _context.SaveChangesAsync();

            return borrowedBook;
        }

        public async Task<BorrowedBook> ReturnBook(BorrowedBook borrowedBook, string remarks, decimal miscFine)
        {
            borrowedBook.DueDate = borrowedBook.DueDate;
            borrowedBook.ActualReturnDate = DateTime.Now;
            borrowedBook.Status = "Returned";


            // Calculate the fine amount
            var fineCalculator = new FineCalculator();
            decimal fineAmount = fineCalculator.CalculateFine(borrowedBook);



            // Find the corresponding invoice
            var invoice = await _context.Invoices.FirstOrDefaultAsync(i => i.BorrowId == borrowedBook.BorrowedBookId);

            if (invoice != null)
            {
                if (borrowedBook.UserInfo?.IsSubscribed == true)
                {
                    invoice.Payment = (fineAmount + miscFine);
                    invoice.Fine = fineAmount;
                    invoice.TransactionDate = DateTime.Now;
                    invoice.Remarks = remarks;
                    invoice.MiscellaneousFines = miscFine;
                }
                else
                {


                    invoice.Refund -= (fineAmount + miscFine);

                    //payment after retuning will be changed to new payment = payment - refund
                    invoice.Payment = invoice.Payment - invoice.Refund;

                    invoice.Fine = fineAmount;
                    invoice.TransactionDate = DateTime.Now;
                    invoice.Remarks = remarks;
                    invoice.MiscellaneousFines = miscFine;
                }
            }
            // without checking user subs
            //if (invoice != null)
            //{
            //    invoice.Refund = invoice.Refund - (fineAmount + miscFine);
            //    invoice.Fine = fineAmount;
            //    invoice.TransactionDate = DateTime.Now;
            //    invoice.Remarks = remarks;
            //    invoice.MiscellaneousFines = miscFine;
            //}

            // Save changes to the database
            await _context.SaveChangesAsync();

            return borrowedBook;
        }
        
        public async Task<BorrowedBook?> Get(int id)
        {
            var borrowedBook = await _context.BorrowedBooks.Include(b => b.UserInfo).Include(bb => bb.Book).FirstOrDefaultAsync(b => b.BorrowedBookId == id);
            return borrowedBook;
        }

        /// <summary>
        /// send request based on subs plan taken
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="borrowedBook"></param>
        /// <returns></returns>
        public async Task<IActionResult> SendBookRequest(string userId, BorrowedBook borrowedBook)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return new NotFoundObjectResult("User not found.");
            }

            var userSubscriptionPlan = await _context.SubscriptionUsers
                .Where(su => su.ApplicationUserId == user.Id)
                .Select(su => su.SubscriptionPlan)
                .FirstOrDefaultAsync();

            decimal maxAllowedBookPrice = userSubscriptionPlan?.MaxAllowedBookPrice ?? 500; // Default to 500 if no subscription plan

            var availableBookCopies = await _context.Copies
                .Where(bc => bc.BookId == borrowedBook.BookId && bc.IsAvailable)
                .FirstOrDefaultAsync();

            if (availableBookCopies == null)
            {
                return new BadRequestObjectResult("All copies are currently borrowed.");
            }

            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == borrowedBook.BookId);

            if (book == null)
            {
                return new NotFoundObjectResult("Book not found.");
            }

            if (book.BookPrice > maxAllowedBookPrice)
            {
                return new BadRequestObjectResult("User's subscription plan does not allow borrowing this book.");
            }

            borrowedBook.UserId = userId;
            borrowedBook.BookCopyId = availableBookCopies.BookCopyId;
            borrowedBook.RequestTimestamp = DateTime.Now;
            borrowedBook.Status = "Requested";
            borrowedBook.IsActive = true;
            borrowedBook.DueDate = null;
            borrowedBook.ActualReturnDate = null;
            borrowedBook.Comment = "";
            borrowedBook.BorrowDate = null;

            _context.BorrowedBooks.Add(borrowedBook);
            availableBookCopies.IsAvailable = false;

            await _context.SaveChangesAsync();

            return new OkObjectResult("Successfully Requested");

        }



        public Task<BorrowedBook?> Post(BorrowedBook borrowedBook)
        {
            //_context.BorrowedBooks.Add(borrowedBook);
            //await _context.SaveChangesAsync();
            //return borrowedBook;
            throw new NotImplementedException();
        }


        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BorrowedBook> Search(string searchString)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BorrowedBook> GetActive()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BorrowedBook> GetInactive()
        {
            throw new NotImplementedException();
        }
        public Task Put(BorrowedBook entities)
        {
            throw new NotImplementedException();
        }
    }
}
