using static System.Reflection.Metadata.BlobBuilder;

namespace TGJBookStoreWithIdentity.Models
{
    public class CartItem
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total
        {
            get { return Quantity * Price; }
        }
        public string Image { get; set; }

        public CartItem()
        {
        }

        public CartItem(Books book)
        {
            ProductId = book.BookId;
            ProductName = book.Title;
            Price = (decimal)book.Price;
            Quantity = 1;
        }
    }
}
