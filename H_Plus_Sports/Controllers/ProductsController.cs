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
    [Route("api/Products")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository products;

        public ProductsController(IProductRepository products)
        {
            this.products = products;
        }

        private async Task<bool> ProductExists(string id)
        {
            return await products.Exists(id);
        }

        [HttpGet]
        [Produces(typeof(DbSet<Product>))]
        public IActionResult GetProduct()
        {
            return new ObjectResult(products.GetAll());
        }

        [HttpGet("{id}")]
        [Produces(typeof(Product))]
        public async Task<IActionResult> GetProduct([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPut("{id}")]
        [Produces(typeof(Product))]
        public async Task<IActionResult> PutProduct([FromRoute] string id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            try
            {
                await products.Update(product);
                return Ok(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExists(id))
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
        [Produces(typeof(Product))]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await products.Add(product);
            }
            catch (DbUpdateException)
            {
                if (!await ProductExists(product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        [HttpDelete("{id}")]
        [Produces(typeof(Product))]
        public async Task<IActionResult> DeleteProduct([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await ProductExists(id))
            {
                return NotFound();
            }

            await products.Remove(id);

            return Ok();
        }
    }
}