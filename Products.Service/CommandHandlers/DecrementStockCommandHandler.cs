using MediatR;
using Products.Repository.Interfaces;
using Products.Service.CommandResults;
using Products.Service.Commands;

namespace Products.Service.CommandHandlers
{
    public class DecrementStockCommandHandler : IRequestHandler<DecrementStockCommand, DecrementStockResult>
    {
        private readonly IProductRepository _repository;

        public DecrementStockCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<DecrementStockResult> Handle(DecrementStockCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);
            if (product == null)
                return DecrementStockResult.NotFoundResult();

            if (product.Stock < request.Quantity)
                return DecrementStockResult.StockUnavailableResult(product.Stock);

            product.Stock -= request.Quantity;
            await _repository.UpdateAsync(product);
            return DecrementStockResult.SuccessResult(product.Stock);
        }
    }
}