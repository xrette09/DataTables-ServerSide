using DataTables_ServerSide.Models.Entities;
using System.Data.Entity;

namespace DataTables_ServerSide.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<TBL_EMPLOYEE> TBL_EMPLOYEEs { get; set; }
        public DbSet<TBL_DEPARTMENT> TBL_DEPARTMENTs { get; set; }

        public AppDbContext() : base("name=AppDbContext")
        {
            // Initialize database connection
        }
    }
}