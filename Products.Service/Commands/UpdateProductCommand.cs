using MediatR;
using Products.Common.Entities;

namespace Products.Service.Commands
{
    public class UpdateProductCommand : IRequest<int>
    {
        public required Product Product { get; set; }
    }
}
