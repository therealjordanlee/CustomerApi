using CustomerApi.Entities;
using CustomerApi.Exceptions;
using CustomerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Repositories
{
    public interface ICustomerRepository
    {
        Task<List<CustomerEntity>> GetAllCustomersAsync();
        Task<List<CustomerEntity>> GetCustomersByNameAsync(string firstName, string lastName);
        Task AddCustomerAsync(CustomerModel customer);
        Task UpdateCustomerAsync(int id, CustomerModel customer);
        Task DeleteCustomerAsync(int id);
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerContext _customerContext;
        public CustomerRepository(CustomerContext customerContext)
        {
            _customerContext = customerContext;
        }

        public async Task<List<CustomerEntity>> GetAllCustomersAsync()
        {
            var customers = await _customerContext.Customers.ToListAsync();
            return customers;
        }

        public async Task<List<CustomerEntity>> GetCustomersByNameAsync(string firstName, string lastName)
        {
            var entities = await _customerContext.Customers
                .Where(x => (!string.IsNullOrEmpty(firstName) ? x.FirstName.Contains(firstName, StringComparison.OrdinalIgnoreCase) : true))
                .Where(x => (!string.IsNullOrEmpty(lastName) ? x.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase) : true))
                .ToListAsync();
            return entities;
        }

        public async Task AddCustomerAsync(CustomerModel customer)
        {
            _customerContext.Add(new CustomerEntity
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                DateOfBirth = customer.DateOfBirth
            });
            await _customerContext.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(int id, CustomerModel customer)
        {
            var entity = await _customerContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if(entity == null)
            {
                throw new CustomerNotFoundException();
            }

            if (entity != null)
            {
                entity.FirstName = customer.FirstName;
                entity.LastName = customer.LastName;
                entity.DateOfBirth = customer.DateOfBirth;
                await _customerContext.SaveChangesAsync();
            }
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _customerContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (customer != null)
            {
                _customerContext.Customers.Remove(customer);
                await _customerContext.SaveChangesAsync();
            }
        }
    }
}
