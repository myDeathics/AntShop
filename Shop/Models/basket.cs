using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class basket
    {
        public int Id { get; set; }
        public int user_Id { get; set; }
        [ForeignKey("user_Id")]
        public user? user { get; set; }
        public List<basket_product> basket_products { get; set; } = new ();
    }
}
