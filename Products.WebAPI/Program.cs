using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Products.Repository.Data;
using Products.Service.BusinessServiceExtensions;
using Products.WebAPI.DTOs;
using Products.WebAPI.Middleware;
using Products.WebAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddServices();
builder.Services.AddScoped<IValidator<ProductDto>, ProductDtoValidator>();
builder.Services.AddScoped<IValidator<int>, StockValidator>();
builder.Services.AddScoped<IValidator<StockLevelRangeDto>, StockLevelRangeDtoValidator>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Register your exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
