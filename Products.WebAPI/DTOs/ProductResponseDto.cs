namespace Products.WebAPI.DTOs
{
    public class ProductResponseDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required int Stock { get; set; }
    }
}
