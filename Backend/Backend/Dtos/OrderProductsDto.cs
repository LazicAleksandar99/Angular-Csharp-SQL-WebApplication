﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dtos
{
    public class OrderProductsDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
        public float Total { get; set; }
    }
}
