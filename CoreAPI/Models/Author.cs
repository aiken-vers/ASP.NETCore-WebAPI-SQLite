using System.ComponentModel.DataAnnotations;

namespace CoreAPI.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
