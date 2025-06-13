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
    public class ProductStockManagementController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<int> _validator;
        private readonly IValidator<StockLevelRangeDto> _rangeValidator;

        public ProductStockManagementController(
             IMediator mediator,
             IValidator<int> validator,
             IValidator<StockLevelRangeDto> rangeValidator)
        {
            _mediator = mediator;
            _validator = validator;
            _rangeValidator = rangeValidator;
        }

        [HttpPut("{id}/decrement-stock/{quantity}")]
        public async Task<IActionResult> DecrementStock([FromRoute] int id, [FromRoute] int quantity)
        {
            var validationResult = _validator.Validate(quantity);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var command = new DecrementStockCommand { Id = id, Quantity = quantity };
            var response = await _mediator.Send(command);

            if (response.NotFound) return NotFound();

            if (response.StockUnavailable)
                return StatusCode(409, new { message = "Stock unavailable.", stock = response.Stock });

            return Ok(new { stock = response.Stock });
        }

        [HttpPut("{id}/add-to-stock/{quantity}")]
        public async Task<IActionResult> AddToStock([FromRoute] int id, [FromRoute] int quantity)
        {
            var validationResult = _validator.Validate(quantity);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var command = new IncrementStockCommand { Id = id, Quantity = quantity };
            var response = await _mediator.Send(command);

            if (response.NotFound) return NotFound();

            return Ok(new { stock = response.Stock });
        }

        [HttpGet("stock-level")]
        public async Task<IActionResult> GetByStockLevel([FromQuery, BindRequired] int min, [FromQuery, BindRequired] int max)
        {
            var dto = new StockLevelRangeDto { Min = min, Max = max };
            var validationResult = _rangeValidator.Validate(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var query = new GetProductsByStockLevelQuery { Min = min, Max = max };
            var products = await _mediator.Send(query);

            var response = products.Select(ProductMapper.ToResponseDto);

            return Ok(response);
        }
    }
}