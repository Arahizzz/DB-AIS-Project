using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DBAIS.Controllers
{
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductsRepository _products;

        public ProductController(ProductsRepository products)
        {
            _products = products;
        }

        [HttpGet("all")]
        public async Task<IActionResult> AllProducts()
        {
            var products = await _products.GetProducts();
            return Ok(products);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            await _products.AddProduct(product);
            return Ok();
        }
    }
}