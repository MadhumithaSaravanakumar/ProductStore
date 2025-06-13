using MediatR;
using Products.Repository.Interfaces;
using Products.Service.CommandResults;
using Products.Service.Commands;

namespace Products.Service.CommandHandlers
{
    public class IncrementStockCommandHandler : IRequestHandler<IncrementStockCommand, IncrementStockResult>
    {
        private readonly IProductRepository _repository;

        public IncrementStockCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IncrementStockResult> Handle(IncrementStockCommand request, CancellationToken cancellationToken)
        {           
            var product = await _repository.GetByIdAsync(request.Id);
            if (product == null)
                return IncrementStockResult.NotFoundResult();

            product.Stock += request.Quantity;
            await _repository.UpdateAsync(product);
            return IncrementStockResult.SuccessResult(product.Stock);
        }
    }
}