using Microsoft.AspNetCore.Mvc;
using TGJBookStoreWithIdentity.Models;
using TGJBookStoreWithIdentity.Models.ViewModel;

namespace TGJBookStoreWithIdentity.Controllers
{
    public class CartController : Controller
    {
        private readonly TGJShopContext _context;

        //create context
        public CartController(TGJShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //use session to get object from json - return list 
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new()
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Quantity * x.Price)
            };

            return View(cartVM);
        }
        //adding item to cart - TKS
        public async Task<IActionResult> Add(int id)
        {
            Books books = await _context.Books.FindAsync(id);

            //create list of cart items - TKS
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            //get product 
            CartItem cartItem = cart.Where(c => c.ProductId == id).FirstOrDefault();

            //check if cart empty 
            if (cartItem == null)
            {
                cart.Add(new CartItem(books));
                books.Quantity--;
                _context.SaveChanges();
            }
            else
            {
                //substract from quantity then add to cart - TKS
                cartItem.Quantity += 1;
                books.Quantity--;
                _context.SaveChanges();
            }

            //set object as json - TKS
            HttpContext.Session.SetJson("Cart", cart);
            //notifications on add item - TKS
            TempData["Success"] = "The product has been added!";

            return Redirect(Request.Headers["Referer"].ToString());
        }

        //substracting  items from cart - TKS
        public async Task<IActionResult> Decrease(int id)
        {
            Books books = await _context.Books.FindAsync(id);

            //create list of cart items - TKS
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            //get product 
            CartItem cartItem = cart.Where(c => c.ProductId == id).FirstOrDefault();

            if (cartItem.Quantity > 1 && books.Quantity > 1)
            {
                //add back to book quantity minus from cart -TKS
                --cartItem.Quantity;
                books.Quantity++;
                _context.SaveChanges();
            }
            else
            {
                cart.RemoveAll(p => p.ProductId == id);
                books.Quantity++;
                _context.SaveChanges();
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            //notifications on decrease item - TKS
            TempData["Success"] = "Product has been Removed";

            return RedirectToAction("Index");
        }

        //remove all from cart - TKS
        public async Task<IActionResult> Remove(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            cart.RemoveAll(p => p.ProductId == id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["Success"] = "The product has been removed!";

            return RedirectToAction("Index");
        }

        //clears cart - TKS
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Index");
        }
    }
}
