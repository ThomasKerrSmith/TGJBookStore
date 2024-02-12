using System.Reflection.Metadata;

namespace TGJBookStoreWithIdentity.Models
{
    public class UserWishlistItem
    {
        public int UserId { get; set; }
        public AppUser AppUser { get; set; }

        public int BookId { get; set; }
        public Books Books { get; set; }
    }
}
