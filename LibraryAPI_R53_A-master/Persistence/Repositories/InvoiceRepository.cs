using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Entities;
using LibraryAPI_R53_A.Core.Interfaces;
using LibraryAPI_R53_A.Helpers;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI_R53_A.Persistence.Repositories
{
    public class InvoiceRepository : IInvoice
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Invoice>> GetAll()
        {
            return await _context.Invoices.Include(i => i.User).ToListAsync();
        }
        public async Task<Invoice?> Get(int id)
        {
            var inv = await _context.Invoices.FindAsync(id);
            return inv;
        }

   
        public async Task<Invoice?> Post(Invoice entity)
        {
            _context.Invoices.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Put(Invoice entities)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Invoice> Search(string searchString)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Invoice> GetActive()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Invoice> GetInactive()
        {
            throw new NotImplementedException();
        }
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

    }
}
