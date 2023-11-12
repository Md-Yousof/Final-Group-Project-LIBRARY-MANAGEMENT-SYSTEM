using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI_R53_A.Persistence.Repositories
{
    public class BookCopyRepository : IBookCopy
    {
        private readonly ApplicationDbContext _context;

        public BookCopyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookCopy>> GetAll()
        {
            return await _context.Copies.ToListAsync();
        }
        public async Task<BookCopy?> Get(int id)
        {
            var copies = await _context.Copies.FindAsync(id);
            return copies;
        }


        public async Task<BookCopy?> Post(BookCopy entity)
        {
            _context.Copies.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Put(BookCopy entities)
        {
            _context.Copies.Update(entities);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var copies = await _context.Copies.FindAsync(id);
            if (copies != null)
            {
                _context.Copies.Remove(copies);
                await _context.SaveChangesAsync();
            }
        }

        public IEnumerable<BookCopy> Search(string searchString)
        {
            var copies = from b in _context.Copies
                         where b.Book.Title.ToLower().Contains(searchString.ToLower())
                         select b;
            return copies.ToList();
        }
        public IEnumerable<BookCopy> GetActive()
        {
            var copies = from b in _context.Copies
                         where b.IsActive == true
                         select b;
            return copies;
        }

        public IEnumerable<BookCopy> GetInactive()
        {
            var copies = from b in _context.Copies
                         where b.IsActive == false
                         select b;
            return copies;
        }

        public async Task<BookCopy> GetAvailable(int? bookId)
        {
            var av = await _context.Copies
                .FirstOrDefaultAsync(c => c.BookId == bookId && c.IsAvailable==true);
            if (av!=null)
            {
                return av;
            }
            return null;
        }


        public IEnumerable<BookCopy> GoodCondition()
        {
            var copies = from b in _context.Copies
                         where b.condition == BookCondition.Good
                         select b;
            return copies;
        }
        public IEnumerable<BookCopy> GetDamaged()
        {
            var copies = from b in _context.Copies
                         where b.condition == BookCondition.Damaged
                         select b;
            return copies;
        }

        public IEnumerable<BookCopy> GotToRepair()
        {
            var copies = from b in _context.Copies
                         where b.condition == BookCondition.ToRepair
                         select b;
            return copies;
        }

        public async Task ChangeAvailability(int id, bool isAvailable)
        {
            var bookCopy = await _context.Copies.FindAsync(id);
            if (bookCopy != null)
            {
                bookCopy.IsAvailable = isAvailable;
                _context.Copies.Update(bookCopy);
                await _context.SaveChangesAsync();
            }
        }


    }
}
