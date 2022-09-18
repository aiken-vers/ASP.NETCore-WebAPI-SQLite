using System.ComponentModel.DataAnnotations;

namespace CoreAPI.Models
{
    public class RequestAuthor
    {
        [Required]
        public string Name { get; set; }
    }
}
