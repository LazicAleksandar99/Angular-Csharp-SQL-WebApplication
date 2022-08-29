using Backend.Dtos;
using System;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface IOrderService
    {
        Task<Object> MakeOrder(MakeOrderDto order, long id);
        Task<Object> GetPendingOrders();
        Task<Object> GetSelectedOrder(long id);
        Task<Object> GetAllOrders();
        Task<Object> GetExecutedOrder(long id);
        Task<Object> AcceptOrder(AcceptDeliveryDto deliverer, long id);
        Task<Object> CurrentOrders(long id);
    }
}
