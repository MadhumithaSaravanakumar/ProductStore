using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Service.Commands;
using Products.Service.Queries;
using Products.WebAPI.Mappers;

namespace Products.WebAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductStockManagementController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<int> _validator;

        public ProductStockManagementController(IMediator mediator, IValidator<int> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [HttpPut("{id}/decrement-stock/{quantity}")]
        public async Task<IActionResult> DecrementStock([FromRoute] int id, [FromRoute] int quantity)
        {
            var validationResult = _validator.Validate(quantity);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            var command = new DecrementStockCommand { Id = id, Quantity = quantity };
            var response = await _mediator.Send(command);
            if (response.Stock < 0) return NotFound();
            return Ok(new { stock = response });
        }

        [HttpPut("{id}/add-to-stock/{quantity}")]
        public async Task<IActionResult> AddToStock([FromRoute] int id, [FromRoute] int quantity)
        {
            var validationResult = _validator.Validate(quantity);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            var command = new IncrementStockCommand { Id = id, Quantity = quantity };
            var response = await _mediator.Send(command);
            if (response.Stock < 0) return NotFound();
            return Ok(new { stock = response });
        }

        [HttpGet("stock-level")]
        public async Task<IActionResult> GetByStockLevel([FromQuery] int min, [FromQuery] int max)
        {
            var query = new GetProductsByStockLevelQuery { Min = min, Max = max };
            var products = await _mediator.Send(query);
            var response = products.Select(ProductMapper.ToResponseDto);
            return Ok(response);
        }
    }
}