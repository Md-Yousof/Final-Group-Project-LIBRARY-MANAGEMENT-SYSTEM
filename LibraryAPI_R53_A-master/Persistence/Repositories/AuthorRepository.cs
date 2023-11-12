using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI_R53_A.Persistence.Repositories
{
    public class AuthorRepository : IAuthor
    {
        private readonly ApplicationDbContext _context;
        public AuthorRepository(ApplicationDbContext db)
        {
            _context = db;
        }

        public async Task<IEnumerable<Author>> GetAll()
        {
            return await _context.Authors.Include(a => a.BookAuthor).ToListAsync();
        }

        public async Task<Author?> Get(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            return author;
        }
        public async Task<Author?> Post(Author entity)
        {
            _context.Authors.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Put(Author entities)
        {
            _context.Authors.Update(entities);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
        }
        public IEnumerable<Author> Search(string searchString)
        {
            var author = from b in _context.Authors

                         where b.FirstName.ToLower().Contains(searchString.ToLower()) || b.LastName.ToLower().Contains(searchString.ToLower())
                         select b;

            return author.ToList();
        }
        public IEnumerable<Author> GetActive()
        {
            var author = from b in _context.Authors
                         where b.IsActive == true
                         select b;
            return author;
        }

        public IEnumerable<Author> GetInactive()
        {
            var author = from b in _context.Authors
                         where b.IsActive == false
                         select b;
            return author;
        }

    }
}
