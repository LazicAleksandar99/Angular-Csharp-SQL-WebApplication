using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    //public enum PaymentStatusEnum {Cash=0,PayPal=1,NotPaid=2 }
    public class Order
    {
        public long Id { get; set; }
        public int Price { get; set; }
        public DateTime DeliveryTime { get; set; }
        public DateTime DeliveryDate { get; set; }
      //  public PaymentStatusEnum PaymentStatus { get; set; }
        public bool SuccessfulDelivery { get; set; }

        public bool OrderAccepted { get; set; }
        public bool CurrentOrdeer { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        ////  public long DelivererId { get; set; }
        ////  public Deliverer Deliverer { get; set; }
        public List<Item> OrderItems { get; set; }

    }
}
