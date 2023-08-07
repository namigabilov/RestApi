using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication4.Entities
{
    public class Product : BaseEntity
    {
        public string Tittle { get; set; }

        public double? Price { get; set; }

        public int? Count { get; set; }

        public string? ImageUrl { get; set; }
    }
}
