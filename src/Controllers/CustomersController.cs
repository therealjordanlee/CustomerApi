using CustomerApi.Entities;
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
        private readonly CustomerContext _customerContext;
        public CustomersController(CustomerContext customerContext)
        {
            _customerContext = customerContext;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var customers = await _customerContext.Customers.ToListAsync();
            return Ok(customers);
        }
    }
}
