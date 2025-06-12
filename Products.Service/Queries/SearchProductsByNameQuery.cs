using MediatR;
using Products.Common.Entities;
using System.Collections.Generic;

namespace Products.Service.Queries
{
    public class SearchProductsByNameQuery : IRequest<IEnumerable<Product>>
    {
        public string Name { get; set; }
    }
}