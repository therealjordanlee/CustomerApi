using Microsoft.EntityFrameworkCore;

namespace CustomerApi.Entities
{
    public class CustomerContext : DbContext
    {
        public DbSet<CustomerEntity> Customers { get; set; }

        public CustomerContext(DbContextOptions options) : base(options)
        {
        }
    }
}