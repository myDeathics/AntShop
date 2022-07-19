using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class basket_product
    {
        public int Id { get; set; }
        public int basket_Id { get; set; }
        public int product_Id { get; set; }

        [ForeignKey("basket_Id")]
        public basket? basket { get; set; }

        [ForeignKey("product_Id")]
        public product? product { get; set; }
    }
}
