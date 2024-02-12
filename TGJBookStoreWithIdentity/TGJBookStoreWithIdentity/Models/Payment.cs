namespace TGJBookStoreWithIdentity.Models
{
    //payment model
    public class Payment
    {
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
    }
    public class ChangeViewModel
    {
        public string ChargeId { get; set; }
    }
}
