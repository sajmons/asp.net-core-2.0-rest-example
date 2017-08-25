using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private MyDbContext _context;

        public ValuesController(MyDbContext dbContext)
        {
            this._context = dbContext;
        }

        [HttpGet()]
        public List<Product> Get()
        {
            return this._context.Products.ToList();
        }

        [HttpGet("{id}")]
        public Product Get(int id)
        {
            return this._context.Products.FirstOrDefault(e => e.Id == id);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            var p = _context.Products.FirstOrDefault(t => t.Id == product.Id);
            if (p != null)
            {
                return BadRequest();
            }

            this._context.Products.Add(product);
            this._context.SaveChanges();

            return Created("Get", product);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Product productIn)
        {
            if (productIn == null || productIn.Id != id)
            {
                return BadRequest();
            }

            var product = _context.Products.FirstOrDefault(t => t.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            product.Stock = productIn.Stock;
            product.Name = productIn.Name;

            _context.Products.Update(product);
            _context.SaveChanges();
            return new OkObjectResult(product);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todo = _context.Products.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Products.Remove(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
