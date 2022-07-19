using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class user
    {

        public int Id { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? nickname { get; set; }
        public string? role { get; set; }
        public List<feedback> feedbacks { get; set; } = new();
        public basket? basket { get; set; }
    }
}
