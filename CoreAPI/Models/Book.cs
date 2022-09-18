using System.ComponentModel.DataAnnotations;

namespace CoreAPI.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public int Pages { get; set; }
    }
}
