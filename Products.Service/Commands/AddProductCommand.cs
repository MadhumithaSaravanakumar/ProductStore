using MediatR;
using Products.Common.Entities;

namespace Products.Service.Commands
{
    public class AddProductCommand : IRequest<int>
    {
        public Product Product { get; set; }
    }
}
