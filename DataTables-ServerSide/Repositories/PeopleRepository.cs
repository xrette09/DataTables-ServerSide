using DataTables_ServerSide.DAL;
using DataTables_ServerSide.Models;
using DataTables_ServerSide.Specification;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace DataTables_ServerSide.Repositories
{
    public class PeopleRepository : Repository<People>, IPeopleRepository
    {
        public PeopleRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}