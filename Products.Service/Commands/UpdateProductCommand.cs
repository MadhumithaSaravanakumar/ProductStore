using MediatR;
using Products.Common.Entities;

namespace Products.Service.Commands
{
    public class UpdateProductCommand : IRequest<int>
    {
        public Product Product { get; set; }
    }
}
