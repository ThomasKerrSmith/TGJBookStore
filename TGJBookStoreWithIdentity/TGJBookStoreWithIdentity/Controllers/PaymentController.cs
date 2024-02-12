using Microsoft.AspNetCore.Mvc;
using Stripe;
using TGJBookStoreWithIdentity.Models;

//controller for stripe payment - TKS
namespace TGJBookStoreWithIdentity.Controllers
{
    public class PaymentController : Controller
    {
        [TempData]
        public string TotalAmount { get; set; }
        public IActionResult Index()
        {
            //create session 
            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            ViewBag.cart = cart;
            ViewBag.DollarAmount = cart.Sum(CartItem => CartItem.Price * CartItem.Quantity);
            //convert dollar amount to cents - totalAmount
            ViewBag.total = Math.Round(ViewBag.DollarAmount, 2) * 100;
            ViewBag.total = Convert.ToInt64(ViewBag.total);
            long total = ViewBag.total;
            TotalAmount = total.ToString();
            return View();

            //add tempData to TotalAmount - this allows it to carry over to processing action method - TKS
            //create viewBag.total - this is to connvert total amount to cents as stripe processes in this way
            //it is then converted to long 
        }
        //Process Order 
        [HttpPost]
        public IActionResult Processing(string stripeToken, string stripeEmail)
        {
            //create customer object - allows you to add customer details
            var Customer = new CustomerCreateOptions
            {
                Email = stripeEmail
            };
            //create charge object - allows you to add charge details 
            var serviceCustomer = new CustomerService();
            Customer customer = serviceCustomer.Create(Customer);
            var Charge = new ChargeCreateOptions
            {
                //add total amount tempData 
                Amount = Convert.ToInt64(TempData["TotalAmount"]),
                Currency = "NZD",
                Description = "Buying Books",
                Source = stripeToken,
                ReceiptEmail = stripeEmail,

            };
            //servive success object - allows you to add success details on proccesing view 
            var serviceSuccess = new ChargeService();
            Charge charge = serviceSuccess.Create(Charge);
            if (charge.Status == "succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;
                ViewBag.AmountPaid = Convert.ToDecimal(charge.Amount) % 100 / 100 + (charge.Amount) / 100;
                ViewBag.BalanceTxId = BalanceTransactionId;
                ViewBag.Customer = customer.Name;
                //return View();
            }

            return View();

            //this method processes the charge via HttpPost
            //once this is done - it moves to processing view to show success 
        }

    }
}
