using Microsoft.EntityFrameworkCore;

namespace Taxonomy.DbModels
{
    public class TaxonomyDbContext(DbContextOptions<TaxonomyDbContext> options) : DbContext(options)
    {
        public DbSet<EmployeeNode> Employees { get; set; }
        public DbSet<CEONode> CEOEmployees { get; set; }
        public DbSet<ManagerNode> ManagerEmployees { get; set; }
        public DbSet<DeveloperNode> DeveloperEmployees { get; set; }
    }
}
