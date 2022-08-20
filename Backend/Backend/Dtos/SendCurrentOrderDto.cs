using System;

namespace Backend.Dtos
{
    public class SendCurrentOrderDto
    {
        public long Id { get; set; }
        public float Price { get; set; }
        public long UserId { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public string OrderStatus { get; set; }
        public string Content { get; set; }
        public DateTime DeliveryTime { get; set; }

    }
}
