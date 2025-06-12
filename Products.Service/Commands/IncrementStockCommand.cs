using MediatR;
using Products.Service.CommandResults;

namespace Products.Service.Commands
{
    public class IncrementStockCommand : IRequest<IncrementStockResult>
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
