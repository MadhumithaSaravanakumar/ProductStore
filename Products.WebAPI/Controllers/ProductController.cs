using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Products.Service.Commands;
using Products.Service.Queries;
using Products.WebAPI.DTOs;
using Products.WebAPI.Mappers;

namespace Products.WebAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<ProductDto> _validator;

        public ProductsController(IMediator mediator, IValidator<ProductDto> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllProductsQuery();
            var products = await _mediator.Send(query);

            var response = products.Select(ProductMapper.ToResponseDto);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var query = new GetProductByIdQuery { Id = id };
            var product = await _mediator.Send(query);

            if (product == null) return NotFound();

            return Ok(ProductMapper.ToResponseDto(product));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto productDto)
        {
            var validationResult = await _validator.ValidateAsync(productDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var product = ProductMapper.ToDomain(productDto);

            var command = new AddProductCommand { Product = product };
            var rowsAffected = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, new { id = product.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ProductDto productDto)
        {
            var validationResult = await _validator.ValidateAsync(productDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var product = ProductMapper.ToDomain(productDto);
            product.Id = id;

            var command = new UpdateProductCommand { Product = product };
            var updatedProductId = await _mediator.Send(command);

            if (updatedProductId == 0) return NotFound();

            return Ok(new { id = updatedProductId });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var command = new DeleteProductCommand { Id = id };
            var deletedProductId = await _mediator.Send(command);

            if (deletedProductId == 0) return NotFound();

            return Ok(new { id = deletedProductId });
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery, BindRequired] string name)
        {
            var query = new SearchProductsByNameQuery { Name = name };
            var products = await _mediator.Send(query);

            var response = products.Select(ProductMapper.ToResponseDto);
            return Ok(response);
        }
    }
}
