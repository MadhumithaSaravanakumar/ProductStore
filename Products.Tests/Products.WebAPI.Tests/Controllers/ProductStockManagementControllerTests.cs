using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Products.Common.Entities;
using Products.Service.Commands;
using Products.Service.Queries;
using Products.WebAPI.Controllers;
using Products.WebAPI.DTOs;
using Products.Service.CommandResults;
using FluentValidation.Results;
using Newtonsoft.Json.Linq;

namespace Products.Tests.Products.WebAPI.Tests.Controllers
{
    public class ProductStockManagementControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IValidator<int>> _validatorMock;
        private readonly Mock<IValidator<StockLevelRangeDto>> _rangeValidatorMock;
        private readonly ProductStockManagementController _controller;

        public ProductStockManagementControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _validatorMock = new Mock<IValidator<int>>();
            _rangeValidatorMock = new Mock<IValidator<StockLevelRangeDto>>();
            _controller = new ProductStockManagementController(
                _mediatorMock.Object,
                _validatorMock.Object,
                _rangeValidatorMock.Object
            );
        }

        [Fact]
        public async Task GivenInvalidQuantity_WhenDecrementStock_ThenReturnsBadRequest()
        {
            // Given
            int id = 1;
            int quantity = -1;
            _validatorMock.Setup(v => v.Validate(quantity)).Returns(new ValidationResult(new[] { new ValidationFailure("Quantity", "Invalid quantity") }));

            // When
            var result = await _controller.DecrementStock(id, quantity);

            // Then
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            var errors = badRequestResult.Value as IEnumerable<string>;
            errors.Should().Contain("Invalid quantity");
        }

        [Fact]
        public async Task GivenProductNotFound_WhenDecrementStock_ThenReturnsNotFound()
        {
            // Given
            int id = 1;
            int quantity = 5;
            _validatorMock.Setup(v => v.Validate(quantity)).Returns(new ValidationResult());
            _mediatorMock.Setup(m => m.Send(It.IsAny<DecrementStockCommand>(), default))
                .ReturnsAsync(new DecrementStockResult { NotFound = true });

            // When
            var result = await _controller.DecrementStock(id, quantity);

            // Then
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GivenStockUnavailable_WhenDecrementStock_ThenReturnsConflict()
        {
            // Given
            int id = 1;
            int quantity = 10;
            _validatorMock.Setup(v => v.Validate(quantity)).Returns(new ValidationResult());
            _mediatorMock.Setup(m => m.Send(It.IsAny<DecrementStockCommand>(), default))
                .ReturnsAsync(new DecrementStockResult { StockUnavailable = true, Stock = 2 });

            // When
            var result = await _controller.DecrementStock(id, quantity);

            // Then
            var conflictResult = result as ObjectResult;
            conflictResult.Should().NotBeNull();
            conflictResult.StatusCode.Should().Be(409);
        }

        [Fact]
        public async Task GivenValidQuantity_WhenDecrementStock_ThenReturnsOkWithStock()
        {
            // Given
            int id = 1;
            int quantity = 3;
            _validatorMock.Setup(v => v.Validate(quantity)).Returns(new ValidationResult());
            _mediatorMock.Setup(m => m.Send(It.IsAny<DecrementStockCommand>(), default))
                .ReturnsAsync(new DecrementStockResult { Success = true, Stock = 7 });

            // When
            var result = await _controller.DecrementStock(id, quantity);

            // Then
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var value = JObject.FromObject(okResult.Value);
            ((int)value["stock"]).Should().Be(7);
        }

        [Fact]
        public async Task GivenInvalidQuantity_WhenAddToStock_ThenReturnsBadRequest()
        {
            // Given
            int id = 1;
            int quantity = -1;
            _validatorMock.Setup(v => v.Validate(quantity)).Returns(new ValidationResult(new[] { new ValidationFailure("Quantity", "Invalid quantity") }));

            // When
            var result = await _controller.AddToStock(id, quantity);

            // Then
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            var errors = badRequestResult.Value as IEnumerable<string>;
            errors.Should().Contain("Invalid quantity");
        }

        [Fact]
        public async Task GivenProductNotFound_WhenAddToStock_ThenReturnsNotFound()
        {
            // Given
            int id = 1;
            int quantity = 5;
            _validatorMock.Setup(v => v.Validate(quantity)).Returns(new ValidationResult());
            _mediatorMock.Setup(m => m.Send(It.IsAny<IncrementStockCommand>(), default))
                .ReturnsAsync(new IncrementStockResult { NotFound = true });

            // When
            var result = await _controller.AddToStock(id, quantity);

            // Then
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GivenValidQuantity_WhenAddToStock_ThenReturnsOkWithStockResult()
        {
            // Given
            int id = 1;
            int quantity = 5;
            _validatorMock.Setup(v => v.Validate(quantity)).Returns(new ValidationResult());
            _mediatorMock.Setup(m => m.Send(It.IsAny<IncrementStockCommand>(), default))
                .ReturnsAsync(new IncrementStockResult { Stock = 15, Success = true, NotFound = false });

            // When
            var result = await _controller.AddToStock(id, quantity);

            // Then
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var value = JObject.FromObject(okResult.Value);
            ((int)value["stock"]).Should().Be(15);
        }       
    }
}