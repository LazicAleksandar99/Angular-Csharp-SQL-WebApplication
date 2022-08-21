using System;

namespace Backend.Dtos
{
    public class CurrentOrderDto
    {
        public long Id { get; set; }
        public float Price { get; set; }
        public long UserId { get; set; }
        public string Comment { get; set; }
        public string OrderStatus { get; set; }
        public DateTime DeliveryTime { get; set; }

    }
}
