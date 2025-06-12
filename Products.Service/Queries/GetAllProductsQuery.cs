using MediatR;
using Products.Common.Entities;

namespace Products.Service.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>>
    {
    }
}
