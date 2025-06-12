using MediatR;
using Products.Common.Entities;

namespace Products.Service.Queries
{
    public class GetProductsByStockLevelQuery : IRequest<IEnumerable<Product>>
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
