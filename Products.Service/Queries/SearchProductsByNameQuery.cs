using MediatR;
using Products.Common.Entities;

namespace Products.Service.Queries
{
    public class SearchProductsByNameQuery : IRequest<IEnumerable<Product>>
    {
        public required string Name { get; set; }
    }
}