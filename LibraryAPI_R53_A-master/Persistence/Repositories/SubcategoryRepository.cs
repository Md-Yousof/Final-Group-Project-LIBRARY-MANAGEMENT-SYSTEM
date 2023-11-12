using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI_R53_A.Persistence.Repositories
{
    public class SubcategoryRepository : IRepository<Subcategory>
    {
        private readonly ApplicationDbContext _context;

        public SubcategoryRepository(ApplicationDbContext context
            )
        {
            _context = context;
        }

        public async Task Delete(int id)
        {
            var subcategory = await _context.Subcategories.FirstOrDefaultAsync(s => s.SubcategoryId == id);
            if (subcategory!=null)
            {
                _context.Subcategories.Remove(subcategory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Subcategory?> Get(int id)
        {
            var subcategory = await _context.Subcategories.FirstOrDefaultAsync(s => s.SubcategoryId == id);

            if (subcategory == null)
            {
                return null;
            }
            return subcategory;
        }

        public IEnumerable<Subcategory> GetActive()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Subcategory>> GetAll()
        {
            return await _context.Subcategories.ToListAsync();
        }

        public IEnumerable<Subcategory> GetInactive()
        {
            throw new NotImplementedException();
        }

        public async Task<Subcategory?> Post(Subcategory sub)
        {

            if (_context.Subcategories.Any(p => p.Name == sub.Name))
            {
                return null;
            }
            _context.Subcategories.Add(sub);
            await _context.SaveChangesAsync();
            return sub;
        }

        public async Task Put(Subcategory sub)
        {
            _context.Entry(sub).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Subcategory> Search(string searchString)
        {
            var subcategory = from p in _context.Subcategories
                            where p.Name.ToLower().Contains(searchString.ToLower())
                            select p;
            return subcategory.ToList();
        }
    }
}
