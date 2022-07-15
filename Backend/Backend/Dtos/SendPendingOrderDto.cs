using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dtos
{
    public class SendPendingOrderDto
    {
        public long Id { get; set; }
        public float Price { get; set; }
        public long UserId { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public string Content { get; set; }

    }
}
