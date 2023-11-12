using AutoMapper;
using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Interfaces;
using LibraryAPI_R53_A.Core.Repositories;
using LibraryAPI_R53_A.DTOs;
using LibraryAPI_R53_A.Helpers;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI_R53_A.Persistence.Repositories
{
    public class BookRepository : IBook
    {
        private readonly ApplicationDbContext _context;
        

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            return await _context.Books.Include(b => b.BookAuthor).ToListAsync();

        }

        public async Task<Book?> Get(int id)
        {
            var book = await _context.Books.FindAsync(id);
            return book;
        }


        public async Task<Book?> Post(Book entity)
        {
            if (_context.Books.Any(b => b.ISBN == entity.ISBN))
            {
                return null;
            }
            _context.Books.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Put(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
        public IEnumerable<Book> Search(string searchString)
        {
            var book = from b in _context.Books
                            where b.Title.ToLower().Contains(searchString.ToLower())
                            select b;
            return book.ToList();
        }
        public IEnumerable<Book> GetActive()
        {
            var book = from b in _context.Books
                            where b.IsActive == true
                            select b;
            return book;
        }

        public IEnumerable<Book> GetInactive()
        {
            var book = from b in _context.Books
                            where b.IsActive == false
                            select b;
            return book;
        }

        public async Task<Book?> GetByISBN(string isbn)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task UpdateBookAuthors(Book book, IEnumerable<int> authorIds)
        {
            // Clear existing BookAuthor records for this book
            book.BookAuthor.Clear();

            foreach (var authorId in authorIds)
            {
                // Check if the BookAuthor record already exists for this BookId and AuthorId
                var existingBookAuthor = _context.BooksAuthors
                    .FirstOrDefault(ba => ba.BookId == book.BookId && ba.AuthorId == authorId);

                if (existingBookAuthor == null)
                {
                    // If it doesn't exist, create a new one
                    var bookAuthor = new BookAuthor
                    {
                        BookId = book.BookId,
                        AuthorId = authorId
                    };

                    book.BookAuthor.Add(bookAuthor);
                }
                // If it exists, no need to create a duplicate entry
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveAuthorsFromBook(int bookId,IEnumerable<int> authorIdsToRemove)
        {
            var book = await _context.Books
                .Include(b => b.BookAuthor)
                .SingleOrDefaultAsync(b => b.BookId == bookId);

            if (book == null)
            {
                throw new InvalidOperationException("Book not found.");
            }

            foreach (var authorIdToRemove in authorIdsToRemove)
            {
                var bookAuthorToRemove = book.BookAuthor.FirstOrDefault(ba => ba.AuthorId == authorIdToRemove);

                if (bookAuthorToRemove != null)
                {
                    book.BookAuthor.Remove(bookAuthorToRemove);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
