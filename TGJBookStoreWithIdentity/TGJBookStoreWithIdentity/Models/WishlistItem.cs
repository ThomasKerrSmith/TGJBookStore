using Microsoft.CodeAnalysis;
using Stripe;

namespace TGJBookStoreWithIdentity.Models
{
    public class WishlistItem
    {
        public string UserID { get; set; }
        public int BookId { get; set; }

        public WishlistItem(AppUser user)
        {
            UserID = user.Id;
        }

    }
    
}
