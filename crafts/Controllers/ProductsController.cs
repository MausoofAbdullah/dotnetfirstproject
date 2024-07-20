using crafts.Model;
using crafts.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace crafts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(JsonFileProductService productService, ILogger<ProductsController> logger)
        {
            this.ProductService = productService;
            this._logger = logger;
        }
        public JsonFileProductService ProductService { get; }
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return ProductService.GetProducts();
        }

        [Route("Rate")]
        [HttpGet]
        public ActionResult Get(
            [FromQuery] string productId,
            [FromQuery] int rating)
        {
            try
            {
                ProductService.AddRating(productId, rating);
                return Ok(new { Message = "Rating added successfully", ProductId = productId, Rating = rating });
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 status code with error message
                _logger.LogError(ex, "An error occurred while adding rating");
                return StatusCode(500, new { Message = "An error occurred while adding rating", Error = ex.Message });
            }
        }

    }
}
