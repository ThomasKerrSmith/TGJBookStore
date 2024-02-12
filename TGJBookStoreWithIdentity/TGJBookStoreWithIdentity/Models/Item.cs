using static System.Reflection.Metadata.BlobBuilder;

namespace TGJBookStoreWithIdentity.Models
{
    public class Item
    {
        public Books books { get; set; }
        public int Quantity { get; set; }
    }
}
