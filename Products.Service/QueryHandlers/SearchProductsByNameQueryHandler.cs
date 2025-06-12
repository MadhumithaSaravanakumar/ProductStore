using MediatR;
using Products.Common.Entities;
using Products.Repository.Interfaces;
using Products.Service.Queries;

namespace Products.Service.QueryHandlers
{
    public class SearchProductsByNameQueryHandler : IRequestHandler<SearchProductsByNameQuery, IEnumerable<Product>>
    {
        private readonly IProductRepository _repository;

        public SearchProductsByNameQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> Handle(SearchProductsByNameQuery request, CancellationToken cancellationToken)
        {
            var all = await _repository.GetAllAsync();
            return all.Where(p => p.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
