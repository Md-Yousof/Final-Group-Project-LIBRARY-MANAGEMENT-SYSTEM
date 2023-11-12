using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI_R53_A.Persistence.Repositories
{
    public class ShelfRepository : IRepository<Shelf>
    {
        private readonly ApplicationDbContext _context;

        public ShelfRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Shelf?> Get(int id)
        {
            var shelf = await _context.Shelfs.Include(s => s.BookRacks).FirstOrDefaultAsync(s => s.ShelfId == id);

            if (shelf == null)
            {
                return null;
            }
            return shelf;
        }
        public async Task<IEnumerable<Shelf>> GetAll()
        {
            return await _context.Shelfs.Include(s => s.BookRacks).ToListAsync();
        }

        public async Task<Shelf?> Post(Shelf shelf)
        {
            //foreach (var bookRack in shelf.BookRacks)
            //{
            //    _context.BookRacks.Add(bookRack);
            //}

            _context.Shelfs.Add(shelf);
            await _context.SaveChangesAsync();
            return shelf;
        }

        public async Task Put(Shelf shelf)
        {
            var existingShelf = await _context.Shelfs
                .Include(s => s.BookRacks)
                .FirstOrDefaultAsync(s => s.ShelfId == shelf.ShelfId);

            if (existingShelf == null)
            {
                throw new InvalidOperationException("Not Found");
            }

            // Detach the existing entity from the context to prevent conflicts
            _context.Entry(existingShelf).State = EntityState.Detached;

            // Attach the updated entity and mark it as modified
            _context.Shelfs.Update(shelf);

            // Clear bookrack from shelf
            foreach (var exBookrack in existingShelf.BookRacks.ToList())
            {
                if (!shelf.BookRacks.Any(br => br.BookRackId == exBookrack.BookRackId))
                {
                    existingShelf.BookRacks.Remove(exBookrack);
                }
            }

            // Update bookrack
            foreach (var updatedBookRack in shelf.BookRacks)
            {
                var existingBookRack = existingShelf.BookRacks.FirstOrDefault(br => br.BookRackId == updatedBookRack.BookRackId);
                if (existingBookRack != null)
                {
                    _context.Entry(existingBookRack).CurrentValues.SetValues(updatedBookRack);
                }
                else
                {
                    // Add new BookRack
                    existingShelf.BookRacks.Add(updatedBookRack);
                }
            }

            await _context.SaveChangesAsync();
        }


        public async Task Delete(int id)
        {
            //var shelf = await _context.Shelfs.FindAsync(id);
            var shelf = await _context.Shelfs.Include(s => s.BookRacks).FirstOrDefaultAsync(s => s.ShelfId == id);
            if (shelf != null)
            {
                _context.Shelfs.Remove(shelf);
                await _context.SaveChangesAsync();
            }
        }
        public IEnumerable<Shelf> GetActive()
        {
            throw new NotImplementedException();
        }


        public IEnumerable<Shelf> GetInactive()
        {
            throw new NotImplementedException();
        }


        public IEnumerable<Shelf> Search(string searchString)
        {
            throw new NotImplementedException();
        }
    }
}
