using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class product
    {   
        [Key]
        public int Id { get; set; }
        public string? name { get; set; }
        public int price { get; set; }
        public string? image { get; set; }
        public int subtype_Id { get; set; }
        public int type_Id { get; set; }
        [ForeignKey("subtype_Id")]
        public subtype? subtype { get; set; }
        [ForeignKey("type_Id")]
        public type? type { get; set; }
        public List<basket_product> basket_products { get; set; } = new();
        public List<product_info> product_infos { get; set; } = new();
        public List<feedback> feedbacks { get; set; } = new();
    }
}
