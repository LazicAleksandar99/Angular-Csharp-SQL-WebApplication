namespace Backend.Models
{
    public class Item
    {
        public long Id { get; set; }
        public long Quantity { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
        public long OrderId { get; set; }
        public Order Order { get; set; }
    }
}
