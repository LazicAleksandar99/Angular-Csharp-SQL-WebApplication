﻿namespace Backend.Dtos
{
    public class OrderDto
    {
        public long Id { get; set; }
        public float Price { get; set; }
        public long UserId { get; set; }
        public string Comment { get; set; }
        public string OrderStatus { get; set; }

    }
}
