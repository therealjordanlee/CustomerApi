using CustomerApi.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CustomerApi.TestHelpers
{
    public static class RepositoryTestHelper
    {
        public static async Task<CustomerContext> GetInMemoryCustomerDbContext(string dbName)
        {
            var dbContextOptions = new DbContextOptionsBuilder<CustomerContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

            var customerContext = new CustomerContext(dbContextOptions);
            customerContext.AddRange(GetMockCustomerData());
            await customerContext.SaveChangesAsync();
            return customerContext;
        }

        public static CustomerEntity[] GetMockCustomerData()
        {
            return new CustomerEntity[]
            {
                new CustomerEntity{Id=1, FirstName="Jordan", LastName="Lee", DateOfBirth=new DateTime(2005,1,1) },
                new CustomerEntity{Id=2, FirstName="Jordan", LastName="Li", DateOfBirth=new DateTime(1980,1,1) },
                new CustomerEntity{Id=3, FirstName="Andrew", LastName="Hoffman", DateOfBirth=new DateTime(1970,1,1) },
                new CustomerEntity{Id=4, FirstName="Michael", LastName="Hoffman", DateOfBirth=new DateTime(1960,1,1) }
            };
        }
    }
}
