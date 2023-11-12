using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI_R53_A.Persistence.Repositories
{
    public class BookFloorRepository : IRepository<BookFloor>
    {
        private readonly ApplicationDbContext _context;


        public BookFloorRepository(ApplicationDbContext context)
        {
            _context = context;

        }


        public async Task<IEnumerable<BookFloor>> GetAll()
        {
            return await _context.BookFloors.ToListAsync();
        }

        public async Task<BookFloor?> Get(int id)
        {
            var BookFloor = await _context.BookFloors.FindAsync(id);
            return BookFloor;
        }

        public async Task<BookFloor?> Post(BookFloor entity)
        {
            if (_context.BookFloors.Any(p => p.BookFloorName == entity.BookFloorName))
            {
                return null;
            }
            _context.BookFloors.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Put(BookFloor bookFloor)
        {
            var update = new BookFloor();
            update.BookFloorId = bookFloor.BookFloorId;
            update.BookFloorName = bookFloor.BookFloorName;
            update.IsActive = bookFloor.IsActive;

            _context.BookFloors.Update(update);
            await _context.SaveChangesAsync();

        }

        public async Task Delete(int id)
        {
            var BookFloor = await _context.BookFloors.FindAsync(id);
            if (BookFloor != null)
            {
                _context.BookFloors.Remove(BookFloor);
                await _context.SaveChangesAsync();
            }
        }

        public IEnumerable<BookFloor> Search(string query)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BookFloor> GetActive()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BookFloor> GetInactive()
        {
            throw new NotImplementedException();
        }
    }
}
