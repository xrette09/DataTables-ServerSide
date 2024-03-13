using DataTables_ServerSide.DAL;
using DataTables_ServerSide.Models.Entities;
using DataTables_ServerSide.Repositories.Interfaces;

namespace DataTables_ServerSide.Repositories.Implementations
{
    public class EmployeeRepository : BaseRepository<TBL_EMPLOYEE>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}