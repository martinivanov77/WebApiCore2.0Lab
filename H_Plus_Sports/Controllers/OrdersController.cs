using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using H_Plus_Sports.Contracts;
using H_Plus_Sports.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HPlusSportsAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Orders")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository orders;

        public OrdersController(IOrderRepository orders)
        {
            this.orders = orders;
        }

        private async Task<bool> OrderExists(int id)
        {
            return await orders.Exists(id);
        }

        [HttpGet]
        [Produces(typeof(DbSet<Order>))]
        public IActionResult GetOrder()
        {
            return new ObjectResult(orders.GetAll());
        }

        [HttpGet("{id}")]
        [Produces(typeof(Order))]
        public async Task<IActionResult> GetOrder([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPut("{id}")]
        [Produces(typeof(Order))]
        public async Task<IActionResult> PutOrder([FromRoute] int id, [FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.OrderId)
            {
                return BadRequest();
            }

            try
            {
                await orders.Update(order);
                return Ok(order);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpPost]
        [Produces(typeof(Order))]
        public async Task<IActionResult> PostOrder([FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await orders.Add(order);
            }
            catch (DbUpdateException)
            {
                if (!await OrderExists(order.OrderId))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        [HttpDelete("{id}")]
        [Produces(typeof(Order))]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await OrderExists(id))
            {
                return NotFound();
            }

            await orders.Remove(id);

            return Ok();
        }
    }
}
