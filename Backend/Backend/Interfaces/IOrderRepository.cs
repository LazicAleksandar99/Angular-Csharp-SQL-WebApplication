using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface IOrderRepository
    {
        void AddOrder(Order order);

        void AddItem(Item item);
    }
}
