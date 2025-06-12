using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Products.Service.Commands;
using Products.Service.Queries;
using Products.WebAPI.Controllers;
using Products.WebAPI.DTOs;
using Products.Common.Entities;


namespace Products.Tests.Products.WebAPI.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IValidator<ProductDto>> _validatorMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _validatorMock = new Mock<IValidator<ProductDto>>();
            _controller = new ProductsController(_mediatorMock.Object, _validatorMock.Object);
        }

        [Fact(DisplayName = "Given products exist, When GetAll is called, Then returns Ok with products")]
        public async Task Given_ProductsExist_When_GetAll_Then_ReturnsOkWithProducts()
        {
            // Given
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "A", Stock = 10 },
                new Product { Id = 2, Name = "B", Stock = 20 }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            // When
            var result = await _controller.GetAll();

            // Then
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<IEnumerable<ProductResponseDto>>(okResult.Value);
            Assert.Equal(2, response.Count());
        }

        [Fact(DisplayName = "Given product exists, When GetById is called, Then returns Ok with product")]
        public async Task Given_ProductExists_When_GetById_Then_ReturnsOkWithProduct()
        {
            // Given
            var product = new Product { Id = 1, Name = "A", Stock = 10 };
            _mediatorMock.Setup(m => m.Send(It.Is<GetProductByIdQuery>(q => q.Id == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // When
            var result = await _controller.GetById(1);

            // Then
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ProductResponseDto>(okResult.Value);
            Assert.Equal(1, response.Id);
        }

        [Fact(DisplayName = "Given product does not exist, When GetById is called, Then returns NotFound")]
        public async Task Given_ProductDoesNotExist_When_GetById_Then_ReturnsNotFound()
        {
            // Given
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);

            // When
            var result = await _controller.GetById(99);

            // Then
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Given valid product, When Create is called, Then returns CreatedAtAction")]
        public async Task Given_ValidProduct_When_Create_Then_ReturnsCreatedAtAction()
        {
            // Given
            var dto = new ProductDto { Name = "A", Stock = 5 };
            _validatorMock.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mediatorMock.Setup(m => m.Send(It.IsAny<AddProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // When
            var result = await _controller.Create(dto);

            // Then
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdResult.ActionName);
        }

        [Fact(DisplayName = "Given invalid product, When Create is called, Then returns BadRequest")]
        public async Task Given_InvalidProduct_When_Create_Then_ReturnsBadRequest()
        {
            // Given
            var dto = new ProductDto { Name = "", Stock = -1 };
            var failures = new List<ValidationFailure> { new ValidationFailure("Name", "Name is required.") };
            _validatorMock.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(failures));

            // When
            var result = await _controller.Create(dto);

            // Then
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Name is required.", badRequest.Value as IEnumerable<string>);
        }

        [Fact(DisplayName = "Given valid update, When Update is called, Then returns Ok with id")]
        public async Task Given_ValidUpdate_When_Update_Then_ReturnsOkWithId()
        {
            // Given
            var dto = new ProductDto { Name = "A", Stock = 5 };
            _validatorMock.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // When
            var result = await _controller.Update(1, dto);

            // Then
            var okResult = Assert.IsType<OkObjectResult>(result);
            // Use pattern matching to extract the id property from the anonymous object
            var value = okResult.Value;
            var idProperty = value.GetType().GetProperty("id");
            Assert.NotNull(idProperty);
            var idValue = idProperty.GetValue(value);
            Assert.Equal(1, idValue);
        }

        [Fact(DisplayName = "Given update for non-existing product, When Update is called, Then returns NotFound")]
        public async Task Given_NonExistingProduct_When_Update_Then_ReturnsNotFound()
        {
            // Given
            var dto = new ProductDto { Name = "A", Stock = 5 };
            _validatorMock.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            // When
            var result = await _controller.Update(99, dto);

            // Then
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Given valid delete, When Delete is called, Then returns Ok with id")]
        public async Task Given_ValidDelete_When_Delete_Then_ReturnsOkWithId()
        {
            // Given
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // When
            var result = await _controller.Delete(1);

            // Then
            var okResult = Assert.IsType<OkObjectResult>(result);
            // Use pattern matching to extract the id property from the anonymous object
            var value = okResult.Value;
            var idProperty = value.GetType().GetProperty("id");
            Assert.NotNull(idProperty);
            var idValue = idProperty.GetValue(value);
            Assert.Equal(1, idValue);
        }

        [Fact(DisplayName = "Given delete for non-existing product, When Delete is called, Then returns NotFound")]
        public async Task Given_NonExistingProduct_When_Delete_Then_ReturnsNotFound()
        {
            // Given
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            // When
            var result = await _controller.Delete(99);

            // Then
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Given products match search, When Search is called, Then returns Ok with products")]
        public async Task Given_ProductsMatchSearch_When_Search_Then_ReturnsOkWithProducts()
        {
            // Given
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Match", Stock = 10 }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<SearchProductsByNameQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            // When
            var result = await _controller.Search("Match");

            // Then
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<IEnumerable<ProductResponseDto>>(okResult.Value);
            Assert.Single(response);
        }
    }
}