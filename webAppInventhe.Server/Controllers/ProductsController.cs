using webAppInventhe.Shared.Model;
using webAppInventhe.Shared.Exception;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using webAppInventhe.Shared;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace webAppInventhe.Server.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IRepository _repository;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IRepository repository, ILogger<ProductsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<Products>> GetProducts()
        {
            using (_logger.BeginScope($"{nameof(GetProducts)}"))
            {
                _logger.LogInformation($"Calling method {nameof(GetProducts)}");
            }
            return await _repository.GetProducts();
        }

        [HttpPut("[action]")]
        public async Task PutProducts([FromBody] Products products)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException($"Invalid products {products.Name} input in route {nameof(PutProducts)}");
            }
            using (_logger.BeginScope($"{nameof(PutProducts)} {products.Name}"))
            {
                _logger.LogInformation($"Calling method {nameof(PutProducts)}");
            }
            await _repository.UpdateProduct(products);
        }

        [HttpPost("[action]")]
        public async Task CreateProducts([FromBody] Products products)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException($"Invalid products {products.Name} input in route {nameof(CreateProducts)}");
            }
            using (_logger.BeginScope($"{nameof(CreateProducts)} {products.Name}"))
            {
                _logger.LogInformation($"Calling method {nameof(CreateProducts)}");
            }
            await _repository.UpdateProduct(products);
        }

        [HttpDelete("[action]/{name}")]
        public async Task DeleteProducts(string name)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException($"Invalid products {name} input in route {nameof(DeleteProducts)}");
            }
            using (_logger.BeginScope($"{nameof(DeleteProducts)} {name}"))
            {
                _logger.LogInformation($"Calling method {nameof(DeleteProducts)}");
            }
            await _repository.DeleteProduct(name);
        }
    }
}
