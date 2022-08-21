using System;
using System.Collections.Generic;

namespace Backend.Models
{
    public class Order
    {
        public long Id { get; set; }
        public float Price { get; set; }
        public DateTime DeliveryTime { get; set; }
        public string PaymentStatus { get; set; } //Cash, PayPalPaid, PayPalNotPaid
        public string OrderStatus { get; set; }//Pending,Delivering,Delivered
        public string Comment { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long Deliverer { get; set; }
        public List<Item> OrderItems { get; set; }

    }
}
