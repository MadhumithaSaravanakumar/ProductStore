using System.ComponentModel.DataAnnotations;

namespace Products.Common.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public required int Stock { get; set; }

        public byte[]? RowVersion { get; set; }
    }
}
