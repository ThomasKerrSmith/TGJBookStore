namespace TGJBookStoreWithIdentity.Models.ViewModel
{
    //create cart view model
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
