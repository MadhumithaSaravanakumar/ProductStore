using MediatR;
using Products.Repository.Interfaces;
using Products.Service.Commands;

namespace Products.Service.CommandHandlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
    {
        private readonly IProductRepository _repository;

        public UpdateProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            return await _repository.UpdateAsync(request.Product);
        }
    }
}