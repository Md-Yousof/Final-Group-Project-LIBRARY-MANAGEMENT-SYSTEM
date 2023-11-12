using AutoMapper;
using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Interfaces;
using LibraryAPI_R53_A.Core.Repositories;
using LibraryAPI_R53_A.DTOs;
using LibraryAPI_R53_A.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LibraryAPI_R53_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBook _b;
        private readonly IMapper _mapper;
        private readonly FileHelper _fileHelper; // Add FileHelper
        private readonly IWebHostEnvironment _environment;

        public BookController(IBook books, IMapper mapper, IWebHostEnvironment environment)
        {
            _b = books;
            _mapper = mapper;
            _environment = environment;
            _fileHelper = new FileHelper(Path.Combine(_environment.WebRootPath, "uploads")); // Specify the uploads folder path
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var book = await _b.GetAll();
            return Ok(book);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _b.Get(id);
            return Ok(book);
        }



        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromForm] BookDto model)
        {
            if (model == null)
            {
                return BadRequest("Invalid data.");
            }

            // Used AutoMapper to map the DTO to the entity
            var book = _mapper.Map<Book>(model);


            book.CoverFileName = _fileHelper.SaveFile(model.Cover);
            book.EBookFileName = _fileHelper.SaveFile(model.EBook);
            book.AgreementFileName = _fileHelper.SaveFile(model.AgreementBookCopy);

            foreach (var authorId in model.AuthorIds)
            {
                var bookAuthor = new BookAuthor
                {
                    BookId = book.BookId,
                    AuthorId = authorId
                };

                // Add the bookAuthor to the database
                book.BookAuthor.Add(bookAuthor);
            }


            await _b.Post(book);
            return Ok(book);
        }

        [HttpPut("{isbn}")]
        public async Task<IActionResult> PutBook(string isbn, [FromForm] BookDto model)
        {
            if (model == null)
            {
                return BadRequest("Invalid data.");
            }


            if (isbn != model.ISBN)
            {
                return BadRequest("ISBN  does not match.");
            }

            // Retrieve the existing book based on ISBN
            var existingBook = await _b.GetByISBN(isbn);

            // Check if the book with the given ISBN exists
            if (existingBook == null)
            {
                return NotFound("Book not found.");
            }


            _mapper.Map(model, existingBook);



            if (model.Cover != null)
            {
                existingBook.CoverFileName = _fileHelper.SaveFile(model.Cover);

            }

            if (model.EBook != null)
            {
                existingBook.EBookFileName = _fileHelper.SaveFile(model.EBook);

            }

            if (model.AgreementBookCopy != null)
            {
                existingBook.AgreementFileName = _fileHelper.SaveFile(model.AgreementBookCopy);

            }

            // Update BookAuthor records
            if (model.AuthorIds != null)
            {
                await _b.UpdateBookAuthors(existingBook, model.AuthorIds);
            }


            // Remove authors from the book if specified
            if (model.AuthorIdsToRemove != null && model.AuthorIdsToRemove.Any())
            {

                await _b.RemoveAuthorsFromBook(existingBook.BookId, model.AuthorIdsToRemove);
            }



            await _b.Put(existingBook);

            return Ok(existingBook);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            await _b.Delete(id);
            return Ok(new { message = "Deleted successfully" });
        }

        [HttpGet, Route("SearchBook/{searchString}")]
        public IActionResult SearchByBookName(string searchString)
        {
            var book = _b.Search(searchString);
            return Ok(book);
        }

        [HttpGet, Route("GetActiveBook")]
        public IActionResult GetActiveBook()
        {
            var book = _b.GetActive();
            return Ok(book);
        }

        [HttpGet, Route("GetInactiveBooks")]
        public IActionResult GetInactiveBook()
        {
            var book = _b.GetInactive();
            return Ok(book);
        }
    }
}
