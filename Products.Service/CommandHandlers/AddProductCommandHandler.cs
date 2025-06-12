using MediatR;
using Products.Repository.Interfaces;
using Products.Service.Commands;
using Products.Utility.Generator;

namespace Products.Service.CommandHandlers
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, int>
    {
        private readonly IProductRepository _repository;

        public AddProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            request.Product.Id = await UniqueIdentifierGenerator.GenerateUniqueIdAsync(_repository);
            return await _repository.AddAsync(request.Product);
        }
    }
}
