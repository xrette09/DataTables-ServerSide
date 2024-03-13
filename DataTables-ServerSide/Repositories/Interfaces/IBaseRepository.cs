using DataTables_ServerSide.Models.Entities;
using DataTables_ServerSide.Specification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataTables_ServerSide.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> 
        where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetListAsync();
        Task<IEnumerable<TEntity>> GetListAsync(ISpecification<TEntity> spec);
        Task<IEnumerable<TEntity>> GetListBySpec(ISpecification<TEntity> specification);
    }
}
