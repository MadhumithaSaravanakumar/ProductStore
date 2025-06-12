using System.ComponentModel.DataAnnotations;

namespace Products.WebAPI.DTOs
{
    public class ProductDto
    {       
        public required string Name { get; set; }
       
        public string? Description { get; set; }
  
        public required int Stock { get; set; }
    }
}
