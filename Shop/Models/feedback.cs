using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class feedback
    {
        public int Id { get; set; }
        public int user_Id { get; set; }
        public string? description { get; set; }
        public byte rating { get; set; }
        public string? date { get; set; }
        [ForeignKey("user_Id")]
        public user? user { get; set; }
    }
}
