using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using H_Plus_Sports.Contracts;
using H_Plus_Sports.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace H_Plus_Sports.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;

        //Constructor injection
        public CustomersController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }
        private async Task<bool> CustomerExists(int id)
        {
            return await customerRepository.Exist(id: id);
        }

        [HttpGet]
        [Produces(typeof(DbSet<Customer>))]
        public IActionResult GetCustomer()
        {
            var results = new ObjectResult(customerRepository.GetAll())
            {
                StatusCode = (int)HttpStatusCode.OK
            };
            Request.HttpContext.Response.Headers.Add("X-Total-Count", customerRepository.GetAll().Count().ToString());

            return results;
        }

        [HttpGet("{id}")]
        [Produces(typeof(Customer))]
        public async Task<IActionResult> GetCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var customer = await customerRepository.Find(id);

            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }
        [HttpPost]
        [Produces(typeof(Customer))]
        public async Task<IActionResult> PostCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await customerRepository.Add(customer);

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        [HttpPut("{id}")]
        [Produces(typeof(Customer))]
        public async Task<IActionResult> PutCustomer([FromRoute] int id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if(id!= customer.CustomerId)
            {
                return BadRequest();
            }

            try
            {
                await customerRepository.Update(customer);
                return Ok(customer);
            }
            catch
            {
                if(!await CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        [HttpDelete("{id}")]
        [Produces(typeof(Customer))]
        public async Task<IActionResult> DeleteCustomer([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(! await CustomerExists(id))
            {
                return NotFound();
            }

            await customerRepository.Remove(id);

            return Ok();
        }
    }
}
