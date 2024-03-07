using DataTables_ServerSide.Models;
using System.Data.Entity;

namespace DataTables_ServerSide.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<People> Peoples { get; set; }

        public AppDbContext() : base("name=AppDbContext")
        {
            // Initialize database connection
        }
    }
}