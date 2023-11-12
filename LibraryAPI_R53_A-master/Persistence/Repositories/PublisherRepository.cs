using AutoMapper;
using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Repositories;
using LibraryAPI_R53_A.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryAPI_R53_A.Persistence.Repositories
{
    public class PublisherRepository : IPublisher
    {
        private readonly ApplicationDbContext _context;


        public PublisherRepository(ApplicationDbContext context)
        {
            _context = context;

        }


        public async Task<IEnumerable<Publisher>> GetAll()
        {
            return await _context.Publishers.ToListAsync();
        }

        public async Task<Publisher?> Get(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            return publisher;
        }

        public async Task<Publisher?> Post(Publisher entity)
        {
            if (_context.Publishers.Any(p => p.PublisherName == entity.PublisherName))
            {
                return null;
            }
            _context.Publishers.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Put(Publisher publisher)
        {
            _context.Publishers.Update(publisher);
            await _context.SaveChangesAsync();

        }

        public async Task Delete(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
                await _context.SaveChangesAsync();
            }
        }

       

        public IEnumerable<Publisher> Search(string searchString)
        {
            var publisher = from p in _context.Publishers
                        where p.PublisherName.ToLower().Contains(searchString.ToLower())
                        select p;
            return publisher.ToList();
        }
        public IEnumerable<Publisher> GetActive()
        {
            var publisher = from b in _context.Publishers
                       where b.IsActive == true
                       select b;
            return publisher;
        }

        public IEnumerable<Publisher> GetInactive()
        {
            var publisher = from b in _context.Publishers
                       where b.IsActive == false
                       select b;
            return publisher;
        }
    }
}
