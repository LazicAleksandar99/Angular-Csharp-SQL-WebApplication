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

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await dsdc.Orders.ToListAsync();
        }

        //history of user
        public async Task<IEnumerable<Order>> GetExecutedOrders(long id,string role)
        {//Deliverer = 1, NormalUser = 2
            if (role == "NormalUser")
                return await dsdc.Orders.Where(x => x.UserId == id && x.OrderStatus == "Delivered").ToListAsync();
            else 
                return await dsdc.Orders.Where(x => x.Deliverer == id && x.OrderStatus == "Delivered").ToListAsync();
        }

        public async Task<string> GetOrderItems(long id)
        {
            string ret = "";

            var items = await dsdc.Items.Where(x => x.OrderId == id).ToListAsync();

            foreach(var item in items)
            {
                var product = await dsdc.Products.Where(x => x.Id == item.ProductId).FirstAsync();
                ret += item.Quantity + " - " + product.Name + ",";
            }

            return ret;
        }

        //current orders
        public async Task<IEnumerable<Order>> GetExecutingOrders(long id,string role)
        {
            if (role == "NormalUser")
                return await dsdc.Orders.Where(x => x.UserId == id && x.OrderStatus == "Delivering").ToListAsync();
            else
                return await dsdc.Orders.Where(x => x.Deliverer == id && x.OrderStatus == "Delivering").ToListAsync();
        }

        public async Task<Order> GetOrder(long id)
        {
            return await dsdc.Orders.Where(x => x.Id == id).FirstAsync();
        }
        
        public void UpdateStatus()
        {
            var orders = dsdc.Orders.Where(x => x.OrderStatus == "Delivering").ToList();

            foreach(var order in orders)
            {
                if (order.DeliveryTime < DateTime.Now)
                    order.OrderStatus = "Delivered";
            }
        }
        public async Task<bool> ChekIfDelivererIsWorking(long id)
        {
            return await dsdc.Orders.AnyAsync(x => x.OrderStatus == "Delivering" && x.Deliverer == id);
        }

        public async Task<bool> ChekIfDelivererIsVerifed(long id)
        {
            return await dsdc.Users.AnyAsync(x => x.Verification == "Verified");
        }
    }
}
