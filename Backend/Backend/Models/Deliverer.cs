using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public enum ValidationType { validating = 0, accepted = 1, denied = 2}
    public class Deliverer : Person
    {
        public ValidationType Verified { get; set; }
    //    public Order CurrentDelivery { get; set; }
    //    public List<Order> DelivererHistory { get; set; }

    }
}
