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

namespace Products.Tests.Products.WebAPI.Tests.Controllers
{
    public class ProductStockManagementControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IValidator<int>> _validatorMock;
        private readonly ProductStockManagementController _controller;

        public ProductStockManagementControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _validatorMock = new Mock<IValidator<int>>();
            _controller = new ProductStockManagementController(_mediatorMock.Object, _validatorMock.Object);
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
        public async Task GivenValidQuantity_WhenDecrementStockAndStockIsNegative_ThenReturnsNotFound()
        {
            // Given
            int id = 1;
            int quantity = 5;
            _validatorMock.Setup(v => v.Validate(quantity)).Returns(new ValidationResult());
            _mediatorMock.Setup(m => m.Send(It.IsAny<DecrementStockCommand>(), default))
                .ReturnsAsync(new DecrementStockResult { Stock = -1 });

            // When
            var result = await _controller.DecrementStock(id, quantity);

            // Then
            result.Should().BeOfType<NotFoundResult>();
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
        public async Task GivenValidQuantity_WhenAddToStockAndStockIsNegative_ThenReturnsNotFound()
        {
            // Given
            int id = 1;
            int quantity = 5;
            _validatorMock.Setup(v => v.Validate(quantity)).Returns(new ValidationResult());
            _mediatorMock.Setup(m => m.Send(It.IsAny<IncrementStockCommand>(), default))
                .ReturnsAsync(new IncrementStockResult { Stock = -1 });

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

            // Serialize and deserialize to access anonymous type
            var json = System.Text.Json.JsonSerializer.Serialize(okResult.Value);
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            var stockElement = doc.RootElement.GetProperty("stock");
            var incrementStockResult = System.Text.Json.JsonSerializer.Deserialize<IncrementStockResult>(stockElement.GetRawText());

            incrementStockResult.Should().NotBeNull();
            incrementStockResult.Stock.Should().Be(15);
            incrementStockResult.Success.Should().BeTrue();
            incrementStockResult.NotFound.Should().BeFalse();
        }

        [Fact]
        public async Task GivenStockRange_WhenGetByStockLevel_ThenReturnsOkWithProducts()
        {
            // Given
            int min = 10;
            int max = 100;
            var products = new List<Product> { new Product { Id = 1, Name = "Sample", Stock = 50 } };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductsByStockLevelQuery>(), default))
                .ReturnsAsync(products);

            // When
            var result = await _controller.GetByStockLevel(min, max);

            // Then
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as IEnumerable<ProductResponseDto>;
            response.Should().NotBeNull();
            response.Should().HaveCount(1);
        }
    }
}