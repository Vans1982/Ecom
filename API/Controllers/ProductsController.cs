
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> productsRepo;
        private readonly IGenericRepository<ProductBrand> productBrandRepo;
        private readonly IGenericRepository<ProductType> productTypeRepo;
        private readonly IMapper mapper;

        public ProductsController(IGenericRepository<Product> productsRepo, 
        IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> 
        productTypeRepo, IMapper mapper)
        {
            this.Mapper = mapper;
            this.productsRepo = productsRepo;
            this.productBrandRepo = productBrandRepo;
            this.productTypeRepo = productTypeRepo;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<ProductsToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await this.productsRepo.ListAsync(spec);

            return products.Select(product => new ProductsToReturnDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name
            }).ToList();
        }
        [HttpGet("{id}")]

        public async Task<ActionResult<ProductsToReturnDto>> GetProduct( int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await this.productsRepo.GetEntityWithSpec(spec);
            return this.mapper.Map<Prodict,ProductsToReturnDto>(product);

        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await this.productBrandRepo.ListAllAsync());
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await this.productTypeRepo.ListAllAsync());
        }
    }
}