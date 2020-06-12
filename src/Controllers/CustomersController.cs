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
        private readonly ICustomerRepository _customerRepository;
        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Find a customer by firstname and lastname. Returns all customers if no query is passed.
        /// </summary>
        /// <param name="firstNameIncludes">Search for customer with first name including this string</param>
        /// <param name="lastNameIncludes">Search for customer with last name including this string</param>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> Get([FromQuery]string? firstNameIncludes, [FromQuery]string? lastNameIncludes)
        {
            if (string.IsNullOrEmpty(firstNameIncludes) && string.IsNullOrEmpty(lastNameIncludes)) // return all customers if query parameters are not specified
            {
                var customers = await _customerRepository.GetAllCustomersAsync();
                return Ok(customers);
            }
            else
            {
                var customers = await _customerRepository.GetCustomersByNameAsync(firstNameIncludes, lastNameIncludes);
                return Ok(customers);
            }
        }

        /// <summary>
        /// Create a new customer based on customer details defined in request body.
        /// </summary>
        /// <param name="customer">Customer details</param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<IActionResult> CreateCustomer([FromBody]CustomerModel customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _customerRepository.AddCustomerAsync(customer);
            return Ok();
        }

        /// <summary>
        /// Update an existing customer based on customer details defined in request body
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customer">Customer details</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer([FromRoute] int id,
            [FromBody]CustomerModel customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _customerRepository.UpdateCustomerAsync(id, customer);
            return Ok();
        }

        /// <summary>
        /// Delete an existing customer based on id
        /// </summary>
        /// <param name="id">Id of the customer to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute]int id)
        {
            await _customerRepository.DeleteCustomerAsync(id);
            return Ok();
        }
    }
}
