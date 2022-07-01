using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
