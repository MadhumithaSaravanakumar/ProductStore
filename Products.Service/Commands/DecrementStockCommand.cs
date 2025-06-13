using MediatR;
using Products.Service.CommandResults;

namespace Products.Service.Commands
{
    public class DecrementStockCommand : IRequest<DecrementStockResult>
    {
        public required int Id { get; set; }
        public required int Quantity { get; set; }
    }
}