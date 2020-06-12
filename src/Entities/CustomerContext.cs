using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
