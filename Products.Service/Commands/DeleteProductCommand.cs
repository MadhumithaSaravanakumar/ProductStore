using MediatR;

namespace Products.Service.Commands
{
    public class DeleteProductCommand : IRequest<int>
    {
        public required int Id { get; set; }
    }
}