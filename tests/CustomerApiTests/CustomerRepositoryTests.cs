using CustomerApi.Entities;
using CustomerApi.Exceptions;
using CustomerApi.Models;
using CustomerApi.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CustomerApiTests
{
    public class CustomerRepositoryTests
    {
        public CustomerRepositoryTests()
        {
        }
        [Fact]
        public async Task GetAllCustomersAsync_Returns_AllCustomersFromDb()
        {
            // Arrange
            using (var context = await GetInMemoryCustomerDbContext("GetTest1Db"))
            {
                var customerRepository = new CustomerRepository(context);
                var expectedResult = GetMockCustomerData().ToList();

                //Act
                var result = await customerRepository.GetAllCustomersAsync();

                // Assert
                expectedResult.Should().BeEquivalentTo(result);
            }
        }

        [Fact]
        public async Task GetCustomersByNameAsync_WithFirstNameAndLastName_ReturnsRecordsContainingFirstNameAndLastNameParameters()
        {
            // Arrange
            using (var context = await GetInMemoryCustomerDbContext("GetTest2Db"))
            {
                var customerRepository = new CustomerRepository(context);
                var expectedResult = new List<CustomerEntity>
                {
                    new CustomerEntity{Id=1, FirstName="Jordan", LastName="Lee", DateOfBirth=new DateTime(2005,1,1) }
                };

                //Act
                var result = await customerRepository.GetCustomersByNameAsync(firstName: "jor", lastName:"le");

                result.Should().BeEquivalentTo(expectedResult);
            }
        }

        [Fact]
        public async Task AddCustomerAsync_WithValidCustomerModel_AddsNewRecordIntoDb()
        {
            // Arrange
            using (var context = await GetInMemoryCustomerDbContext("AddTest1Db"))
            {
                var customerRepository = new CustomerRepository(context);
                var newCustomer = new CustomerModel
                {
                    FirstName = "Alan",
                    LastName = "Turing",
                    DateOfBirth = new DateTime(1912, 6, 23)
                };

                var expectedResult = GetMockCustomerData()
                    .ToList();
                expectedResult.Add(new CustomerEntity
                    {
                        Id = 5,
                        FirstName = newCustomer.FirstName,
                        LastName = newCustomer.LastName,
                        DateOfBirth = newCustomer.DateOfBirth
                    }
                    );

                // Act
                await customerRepository.AddCustomerAsync(newCustomer);
                var result = await customerRepository.GetAllCustomersAsync();

                // Assert
                result.Should().BeEquivalentTo(expectedResult);
            }
        }

        [Fact]
        public async Task UpdateCustomerAsync_WithExistingId_UpdatesRecordInDb()
        {
            // Arrange
            using (var context = await GetInMemoryCustomerDbContext("UpdateTest1Db"))
            {
                var customerRepository = new CustomerRepository(context);
                var updatedCustomer = new CustomerModel
                {
                    FirstName = "Alan",
                    LastName = "Turing",
                    DateOfBirth = new DateTime(1912, 6, 23)
                };

                var expectedResult = new CustomerEntity
                {
                    Id = 1,
                    FirstName = updatedCustomer.FirstName,
                    LastName = updatedCustomer.LastName,
                    DateOfBirth = updatedCustomer.DateOfBirth
                };
                    
                // Act
                await customerRepository.UpdateCustomerAsync(1, updatedCustomer);
                var result = await customerRepository.GetCustomersByNameAsync(firstName: updatedCustomer.FirstName, lastName: updatedCustomer.LastName);

                // Assert
                result.Should().BeEquivalentTo(expectedResult);
            }
        }

        [Fact]
        public async Task UpdateCustomerAsync_WithInvalidId_ThrowsCustomerNotFoundException()
        {
            // Arrange
            using (var context = await GetInMemoryCustomerDbContext("UpdateTest2Db"))
            {
                var customerRepository = new CustomerRepository(context);
                var updatedCustomer = new CustomerModel
                {
                    FirstName = "Alan",
                    LastName = "Turing",
                    DateOfBirth = new DateTime(1912, 6, 23)
                };

                // Act
                Func<Task> act = async () =>
                {
                    await customerRepository.UpdateCustomerAsync(99, updatedCustomer);
                };

                // Assert
                act.Should().Throw<CustomerNotFoundException>();
                
            }
        }

        [Fact]
        public async Task DeleteCustomerAsync_WithValidId_RemovesRecordFromDatabase()
        {
            // Arrange
            using (var context = await GetInMemoryCustomerDbContext("DeleteTest1Db"))
            {
                var customerRepository = new CustomerRepository(context);
                var expectedResult = GetMockCustomerData().ToList();
                expectedResult.RemoveAt(0);

                // Act
                Func<Task> act = async () =>
                {
                    await customerRepository.DeleteCustomerAsync(99);
                };
                await customerRepository.DeleteCustomerAsync(1);

                // Assert
                act.Should().Throw<CustomerNotFoundException>();
            }
        }

        [Fact]
        public async Task DeleteCustomerAsync_WithInvalidId_ThrowsCustomerNotFoundException()
        {
            // Arrange
            using (var context = await GetInMemoryCustomerDbContext("DeleteTest2Db"))
            {
                var customerRepository = new CustomerRepository(context);
                var expectedResult = GetMockCustomerData().ToList();
                expectedResult.RemoveAt(0);

                // Act
                await customerRepository.DeleteCustomerAsync(1);
                var result = await customerRepository.GetAllCustomersAsync();

                // Assert
                result.Should().BeEquivalentTo(expectedResult);
            }
        }


        private async Task<CustomerContext> GetInMemoryCustomerDbContext(string dbName)
        {
            var dbContextOptions = new DbContextOptionsBuilder<CustomerContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

            var customerContext = new CustomerContext(dbContextOptions);
            customerContext.AddRange(GetMockCustomerData());
            await customerContext.SaveChangesAsync();
            return customerContext;
        }

        private CustomerEntity[] GetMockCustomerData()
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
