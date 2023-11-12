using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Repositories;

namespace LibraryAPI_R53_A.Core.Interfaces
{
    public interface IBook:IRepository<Book>
    {
        public Task<Book?> GetByISBN(string isbn);
        Task UpdateBookAuthors(Book book, IEnumerable<int> authorIds);
        Task RemoveAuthorsFromBook(int bookId, IEnumerable<int> authorIdsToRemove);

    }
}
