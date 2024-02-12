using System.ComponentModel.DataAnnotations;

namespace TGJBookStoreWithIdentity.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        [Required]
        public string BookTitle { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Reviewer { get; set; }
        [Required]
        public int Rating { get; set; }
        public string Description { get; set; }
    }
}
