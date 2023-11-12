using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Interfaces;
using LibraryAPI_R53_A.Persistence.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryAPI_R53_A.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowBookController : ControllerBase
    {
        private readonly IBorrowBook _bR;
        private readonly IBookCopy _bCR;
        private readonly IInvoice _inv;

        public BorrowBookController(IBorrowBook bR, IBookCopy bCR, IInvoice inv)
        {
            _bR = bR;
            _bCR = bCR;
            _inv = inv;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("all-borrowlist")]
        public async Task<IActionResult> GetBorrowedList()
        {

            var requestedBooks = await _bR.GetAll();
            return Ok(requestedBooks);
        }


        //[Authorize(Roles = "Admin")]
        [HttpGet("requested-books/{username}")]
        public async Task<IActionResult> GetRequestedBooksByUserName(string username)
        {
            try
            {

                var requestedBooks = await _bR.GetAllRequestedBooksByUserName(username);

                if (requestedBooks == null)
                {
                    return NotFound("No requested books found for the user.");
                }


                return Ok(requestedBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //[Authorize(Roles = "Admin, User")]
        [HttpGet("all-request/{username}")]
        public async Task<IActionResult> GetAllRequestByUserName(string username)
        {
            try
            {

                var requestedBooks = await _bR.GetAllByUserName(username);

                if (requestedBooks == null)
                {
                    return NotFound("No requested books found for the user.");
                }


                return Ok(requestedBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }



        //[Authorize(Roles = "Admin")]
        [HttpGet("cancelled-books")]
        public async Task<IActionResult> GetCancelledBooks()
        {
            try
            {

                var cancelledBooks = await _bR.GetAllCancelledBooks();
                return Ok(cancelledBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("approved-books")]
        public async Task<IActionResult> GetApprovedBooks()
        {
            try
            {

                var approvedBooks = await _bR.GetAllApprovedBooks();
                return Ok(approvedBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("book-request")]
        public async Task<IActionResult> SendBookRequest(BorrowedBook borrowedBook)
        {
            var uId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(uId))
            {
                return Unauthorized("User not authenticated.");
            }

            return await _bR.SendBookRequest(uId, borrowedBook);
        }



        //[Authorize(Roles = "Admin, User")]
        //[HttpPost("book-request")]
        //public async Task<IActionResult> SendBookRequest(BorrowedBook borrowedBook)
        //{
        //    try
        //    {
        //        var uId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

        //        if (string.IsNullOrEmpty(uId))
        //        {
        //            return Unauthorized("User not authenticated.");
        //        }

        //        var availablebookCopies = await _bCR.GetAvailable(borrowedBook.BookId);

        //        if (availablebookCopies == null)
        //        {
        //            return BadRequest("All Copies are currently borrowed.");
        //        }


        //        borrowedBook = new BorrowedBook
        //        {
        //            UserId = uId,
        //            BookId = borrowedBook.BookId,
        //            BookCopyId = availablebookCopies.BookCopyId,
        //            RequestTimestamp= DateTime.Now,
        //            Status = "Requested",
        //            IsActive = true

        //        };

        //        await _bR.Post(borrowedBook);
        //        await _bCR.ChangeAvailability(availablebookCopies.BookCopyId, false);


        //        return Ok("Succesfully Requested");


        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal Server Error: {ex.Message}");
        //    }
        //}

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> BooksShow()
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) { return BadRequest(); }
            var a = await _bR.GetAllRequestedBooksByUserId(userId);
            return Ok(a);

        }

        //[Authorize(Roles ="Admin")]
        [HttpPut("Approve/{borrowedBookId}")]
        public async Task<IActionResult> ApproveBorrowedBook(int borrowedBookId)
        {
            var borrowedBook = await _bR.Get(borrowedBookId);

            if (borrowedBook == null)
            {
                return NotFound(); 
            }

            await _bR.ApproveBorrowedBookAsync(borrowedBook);
           
            await _bCR.ChangeAvailability(borrowedBook.BookCopyId, false);


            return Ok(borrowedBook); 
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("Cancel/{borrowedBookId}")]
        public async Task<IActionResult> CancelBorrowedBook(int borrowedBookId, string comment)
        {
            var borrowedBook = await _bR.Get(borrowedBookId);

            if (borrowedBook == null)
            {
                return NotFound();
            }

            borrowedBook.Comment = comment;
            await _bR.CancelBorrowedBookAsync(borrowedBook);
            await _bCR.ChangeAvailability(borrowedBook.BookCopyId, true);


            return Ok(borrowedBook);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("Return/{borrowedBookId}")]
        public async Task<IActionResult> ReturnBook(int borrowedBookId, string remarks, decimal miscFine )
        {
            var borrowedBook = await _bR.Get(borrowedBookId);

            if (borrowedBook == null)
            {
                return NotFound();
            }

          
            await _bR.ReturnBook(borrowedBook, remarks, miscFine);
            await _bCR.ChangeAvailability(borrowedBook.BookCopyId, true);

            return Ok(borrowedBook);
        }

        //admin inspect, received and fine book



    }
}
