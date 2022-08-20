
namespace Backend.Models
{
    public enum ValidationType { validating = 0, accepted = 1, denied = 2}
    public class Deliverer
    {
        public ValidationType Verified { get; set; }
    //    public Order CurrentDelivery { get; set; }
    //    public List<Order> DelivererHistory { get; set; }

    }
}
