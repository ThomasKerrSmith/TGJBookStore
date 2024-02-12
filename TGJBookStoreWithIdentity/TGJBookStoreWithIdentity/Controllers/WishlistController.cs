using Microsoft.AspNetCore.Mvc;
using TGJBookStoreWithIdentity.Models.ViewModel;
using TGJBookStoreWithIdentity.Models;
using TGJBookStoreWithIdentity.ViewModels;

namespace TGJBookStoreWithIdentity.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Index()
        {
            List<WishlistItem> wishlistItems = new List<WishlistItem>();

            WishlistViewModel wishlistVM = new()
            {
                //WishlistItems = wishlistItems,
                //UserID = wishlistItems.UserID
            };
            return View(wishlistVM);
        }
    }
}
