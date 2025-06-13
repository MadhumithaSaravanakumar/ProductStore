using MediatR;
using Products.Service.CommandResults;

namespace Products.Service.Commands
{
    public class IncrementStockCommand : IRequest<IncrementStockResult>
    {
        public required int Id { get; set; }
        public required int Quantity { get; set; }
    }
}
