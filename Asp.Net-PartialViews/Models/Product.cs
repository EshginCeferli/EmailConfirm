using System.ComponentModel.DataAnnotations.Schema;

namespace Asp.Net_PartialViews.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public string ProductImage { get; set; }
    }
}
