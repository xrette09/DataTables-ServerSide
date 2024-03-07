using DataTables_ServerSide.Models;
using DataTables_ServerSide.Specification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataTables_ServerSide.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        Task<IEnumerable<T>> GetListAsync();
        Task<IEnumerable<T>> GetListAsync(ISpecification<T> spec);
        Task<IEnumerable<T>> GetListBySpec(ISpecification<T> specification);
    }
}
