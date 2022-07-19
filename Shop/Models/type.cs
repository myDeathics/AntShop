using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class type
    {   
        public int Id { get; set; }
        public string? name { get; set; }
        public List<product> products { get; set; } = new ();
        public List<subtype> subtypes { get; set; } = new ();
    }
}
