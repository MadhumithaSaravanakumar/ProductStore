using MediatR;
using Products.Service.CommandResults;

namespace Products.Service.Commands
{
    public class DecrementStockCommand : IRequest<DecrementStockResult>
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}