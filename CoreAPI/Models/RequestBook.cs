using System.ComponentModel.DataAnnotations;

namespace CoreAPI.Models
{
    public class RequestBook
    {
        [Required]
        public string Title { get; set; }
        public int Pages { get; set; }
    }
}
