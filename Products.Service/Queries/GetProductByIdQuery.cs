using MediatR;
using Products.Common.Entities;

namespace Products.Service.Queries
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public required int Id { get; set; }
    }
}
