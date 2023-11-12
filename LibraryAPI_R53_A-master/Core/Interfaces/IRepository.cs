using System.Linq.Expressions;

namespace LibraryAPI_R53_A.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity?> Get(int id);
        Task<TEntity?> Post(TEntity entity);
        Task Put(TEntity entities);

        Task Delete(int id);

        IEnumerable<TEntity> Search(string searchString);
        IEnumerable<TEntity> GetActive();
        IEnumerable<TEntity> GetInactive();
  

    }
}
