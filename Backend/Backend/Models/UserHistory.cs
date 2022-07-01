using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class UserHistory
    {
        public long Id { get; set; }
        
        public long UserId { get; set; }

        public User User { get; set; }

        public List<Order> AllOrders { get; set; }
    }
}
