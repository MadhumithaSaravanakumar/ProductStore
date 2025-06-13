using MediatR;
using Products.Common.Entities;

namespace Products.Service.Queries
{
    public class GetProductsByStockLevelQuery : IRequest<IEnumerable<Product>>
    {
        public required int Min { get; set; }
        public required int Max { get; set; }
    }
}
