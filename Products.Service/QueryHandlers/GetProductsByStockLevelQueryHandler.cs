using MediatR;
using Products.Common.Entities;
using Products.Repository.Interfaces;
using Products.Service.Queries;

namespace Products.Service.QueryHandlers
{
    public class GetProductsByStockLevelQueryHandler : IRequestHandler<GetProductsByStockLevelQuery, IEnumerable<Product>>
    {
        private readonly IProductRepository _repository;

        public GetProductsByStockLevelQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> Handle(GetProductsByStockLevelQuery request, CancellationToken cancellationToken)
        {
            var all = await _repository.GetAllAsync();
            return all.Where(p => p.Stock >= request.Min && p.Stock <= request.Max);
        }
    }
}