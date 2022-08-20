using Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface IOrderRepository
    {
        void AddOrder(Order order);

        void AddItem(Item item);

        void UpdatePrice(long id, float price);

        Task<IEnumerable<Order>> GetPendingOrders();

        Task<IEnumerable<Order>> GetAllOrders();

        Task<IEnumerable<Order>> GetExecutedOrders(long id,string role);

        Task<string> GetOrderItems(long id);

        Task<IEnumerable<Order>> GetExecutingOrders(long id, string role);

        Task<Order> GetOrder(long id);

        void UpdateStatus();

        Task<bool> ChekIfDelivererIsWorking(long id);

        Task<bool> ChekIfDelivererIsVerifed(long id);
    }
}
