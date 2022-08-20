using System.Collections.Generic;

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
