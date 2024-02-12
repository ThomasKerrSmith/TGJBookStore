using System.ComponentModel.DataAnnotations;

namespace TGJBookStoreWithIdentity.Models
{
    public class Books
    {
        [Key]
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
    }
}
