using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Shop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var product = new Product { Id = 1, Name = "test" };
            return Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = new Product { Id = id, Name = "test" };
            return Ok(product);
        }

        [HttpPost]
        public bool Create([FromBody] string obj)
        {
            return true;
        }
    }
}
