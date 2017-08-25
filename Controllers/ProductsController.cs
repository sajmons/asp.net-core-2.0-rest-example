using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private MyDbContext _dbContext;

        public ProductsController(MyDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet()]
        public List<Product> Get()
        {
            return this._dbContext.Products.ToList();
        }

        [HttpGet("{id}")]
        public Product Get(int id)
        {
            return this._dbContext.Products.FirstOrDefault(e => e.Id == id);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            var p = _dbContext.Products.FirstOrDefault(t => t.Id == product.Id);
            if (p != null)
            {
                return BadRequest();
            }

            this._dbContext.Products.Add(product);
            this._dbContext.SaveChanges();

            return Created("Get", product);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Product productIn)
        {
            if (productIn == null || productIn.Id != id)
            {
                return BadRequest();
            }

            var product = _dbContext.Products.FirstOrDefault(t => t.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            product.Stock = productIn.Stock;
            product.Name = productIn.Name;

            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();
            return new OkObjectResult(product);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todo = _dbContext.Products.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            _dbContext.Products.Remove(todo);
            _dbContext.SaveChanges();
            return new NoContentResult();
        }
    }
}
