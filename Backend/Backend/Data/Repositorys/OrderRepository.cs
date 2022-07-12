using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Repositorys
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DeliverySystemDbContext dsdc;

        public OrderRepository(DeliverySystemDbContext dsdc)
        {
            this.dsdc = dsdc;
        }
        public void AddOrder(Order order)
        {
            dsdc.Orders.Add(order);
        }

        public void AddItem(Item item)
        {
            dsdc.Items.Add(item);
        }

        public void UpdatePrice(long id, float price)
        {
            var order = dsdc.Orders.SingleOrDefault(x => x.Id == id);
            order.Price = price;
        }

        public async Task<IEnumerable<Order>> GetPendingOrders()
        {
            return await dsdc.Orders.Where(x => x.OrderStatus == "Pending").ToListAsync();
        }
    }
}
