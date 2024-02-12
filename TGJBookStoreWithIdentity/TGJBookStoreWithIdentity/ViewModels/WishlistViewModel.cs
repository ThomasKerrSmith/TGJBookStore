using System.ComponentModel.DataAnnotations;
using TGJBookStoreWithIdentity.Models;

namespace TGJBookStoreWithIdentity.ViewModels
{
    public class WishlistViewModel
    {
        [Key]
        public string UserID { get; set; }
        public List<WishlistItem> WishlistItems { get; set; }

    }
}
