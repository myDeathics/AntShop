using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class product_info
    {
        public int Id { get; set; }
        public int product_Id { get; set; }
        public string? description { get; set; }
        public string? title { get; set; }

        [ForeignKey("product_Id")]
        public product? product { get; set; }
    }
}
