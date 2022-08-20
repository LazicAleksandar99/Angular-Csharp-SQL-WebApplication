using System.Collections.Generic;

namespace Backend.Models
{
    public class DelivererHistory
    {
        public long Id { get; set; }

        public long DelivererId { get; set; }

        public Deliverer Deliverer { get; set; }

        public List<Order> AllOrders { get; set; }
    }
}
