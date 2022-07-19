using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class subtype
    {
        public int Id { get; set; }
        public string? name { get; set; }
        public int type_Id { get; set; }
        [ForeignKey("type_Id")]
        public type? type { get; set; }
        public List<product> products { get; set; } = new ();
    }
}
