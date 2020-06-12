using CustomerApi.Entities;
using CustomerApi.Models;
using CustomerApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Controllers
{
    [Route("customers")]
    public class CustomersController : ControllerBase
    {
        // TODO: move customercontext to repository class
        //private readonly CustomerContext _customerContext;
        private readonly ICustomerRepository _customerRepository;
        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get([FromQuery]string? firstNameIncludes, [FromQuery]string? lastNameIncludes)
        {
            if (string.IsNullOrEmpty(firstNameIncludes) && string.IsNullOrEmpty(lastNameIncludes)) // return all customers if query parameters are not specified
            {
                //var customers = await _customerContext.Customers.ToListAsync();
                var customers = await _customerRepository.GetAllCustomersAsync();
                return Ok(customers);
            }
            else
            {
                var customers = await _customerRepository.GetCustomersByNameAsync(firstNameIncludes, lastNameIncludes);
                return Ok(customers);
            }
            //var entities = await _customerContext.Customers
            //    .Where(x => (!string.IsNullOrEmpty(firstNameIncludes) ? x.FirstName.Contains(firstNameIncludes,StringComparison.OrdinalIgnoreCase) : true))
            //    .Where(x => (!string.IsNullOrEmpty(lastNameIncludes) ? x.LastName.Contains(lastNameIncludes, StringComparison.OrdinalIgnoreCase) : true))
            //    .ToListAsync();
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateCustomer([FromBody]CustomerModel customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _customerRepository.AddCustomerAsync(customer);

            //try
            //{
            //    _customerContext.Add(new CustomerEntity
            //    {
            //        FirstName = customer.FirstName,
            //        LastName = customer.LastName,
            //        DateOfBirth = customer.DateOfBirth
            //    });
            //    await _customerContext.SaveChangesAsync();
            //}
            //catch(Exception ex)
            //{
            //    // todo: error handling
            //    Console.WriteLine(ex.Message);
            //}
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer([FromRoute] int id,
            [FromBody]CustomerModel customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _customerRepository.UpdateCustomerAsync(id, customer);

            //var entity = await _customerContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
            //if(entity != null)
            //{
            //    entity.FirstName = customer.FirstName;
            //    entity.LastName = customer.LastName;
            //    entity.DateOfBirth = customer.DateOfBirth;
            //    await _customerContext.SaveChangesAsync();
            //}
            //TODO - handle customer not found
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute]int id)
        {
            await _customerRepository.DeleteCustomerAsync(id);
            //var entity = await _customerContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
            //if(entity != null)
            //{
            //    _customerContext.Customers.Remove(entity);
            //    await _customerContext.SaveChangesAsync();
            //}
            //TODO - handle customer not found
            return Ok();
        }
    }
}
