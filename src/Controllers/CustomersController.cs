using CustomerApi.Entities;
using CustomerApi.Models;
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
        private readonly CustomerContext _customerContext;
        public CustomersController(CustomerContext customerContext)
        {
            _customerContext = customerContext;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get([FromQuery]string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var customers = await _customerContext.Customers.ToListAsync();
                return Ok(customers);
            }
            var entities = await _customerContext.Customers.Where(
            c => c.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase)
            || c.LastName.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToListAsync();

            return Ok(entities);
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateCustomer([FromBody]CustomerModel customer)
        {
            var d = new DateTime(2000, 01, 01);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                _customerContext.Add(new CustomerEntity
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    DateOfBirth = customer.DateOfBirth
                });
                await _customerContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                // todo: error handling
                Console.WriteLine(ex.Message);
            }
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

            var entity = await _customerContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if(entity != null)
            {
                entity.FirstName = customer.FirstName;
                entity.LastName = customer.LastName;
                entity.DateOfBirth = customer.DateOfBirth;
                await _customerContext.SaveChangesAsync();
            }
            //TODO - handle customer not found
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute]int id)
        {
            var entity = await _customerContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if(entity != null)
            {
                _customerContext.Customers.Remove(entity);
                await _customerContext.SaveChangesAsync();
            }
            //TODO - handle customer not found
            return Ok();
        }
    }
}
