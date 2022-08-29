using AutoMapper;
using Backend.Dtos;
using Backend.Errors;
using Backend.Interfaces;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public OrderService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<Object> MakeOrder(MakeOrderDto order, long id)
        {
            ApiError apiError = new ApiError();
            var user = await uow.AccountRepository.GetUserDetails(id);
            if (user == null)
            {
                apiError.ErrorCode = 400;
                apiError.ErrorMessage = "Wrong user id";
                return apiError;
            }
            if (String.IsNullOrWhiteSpace(order.comment))
            {
                apiError.ErrorCode = 400;
                apiError.ErrorMessage = "You need to enter comment";
                return apiError;
            }

            float grandTotal = 0;
            Order newOrder = new Order();
            newOrder.OrderStatus = "Pending";
            newOrder.Deliverer = -1;
            newOrder.Comment = order.comment;
            newOrder.UserId = id;
            newOrder.PaymentStatus = order.payment;

            uow.OrderRepository.AddOrder(newOrder);//
            await uow.SaveAsync();// to dole sa 56om linijom izmjeniti

            foreach (var item in order.orderProducts)
            {
                Item newItem = new Item();
                newItem.Quantity = item.Quantity;
                newItem.ProductId = item.Id;
                newItem.OrderId = newOrder.Id;
                grandTotal += item.Total;
                uow.OrderRepository.AddItem(newItem);
            }

            uow.OrderRepository.UpdatePrice(newOrder.Id, grandTotal);
            await uow.SaveAsync();
            return 201;
        }
        public async Task<Object> GetPendingOrders()
        {
            var orders = await uow.OrderRepository.GetPendingOrders();

            var ordersDtos = mapper.Map<IEnumerable<PendingOrderDto>>(orders);
            List<SendPendingOrderDto> pendingOrders = new List<SendPendingOrderDto>();

            foreach (var orderDto in ordersDtos)
            {
                SendPendingOrderDto order = new SendPendingOrderDto();
                order.Id = orderDto.Id;
                order.Price = orderDto.Price;
                order.UserId = orderDto.UserId;
                order.Comment = orderDto.Comment;
                order.Content = await uow.OrderRepository.GetOrderItems(orderDto.Id);
                var user = await uow.AccountRepository.GetUserDetails(orderDto.UserId);
                order.Address = user.Address;
                order.Email = user.Email;
                pendingOrders.Add(order);
            }
            return pendingOrders;
        }
        public async Task<Object> GetSelectedOrder(long id)
        {
            var order = await uow.OrderRepository.GetSelectedOrder(id);

            var ordersDtos = mapper.Map<PendingOrderDto>(order);
            SendPendingOrderDto selectedOrder = new SendPendingOrderDto();

            selectedOrder.Id = ordersDtos.Id;
            selectedOrder.Price = ordersDtos.Price;
            selectedOrder.UserId = ordersDtos.UserId;
            selectedOrder.Comment = ordersDtos.Comment;
            selectedOrder.Content = await uow.OrderRepository.GetOrderItems(ordersDtos.Id);
            var user = await uow.AccountRepository.GetUserDetails(ordersDtos.UserId);
            selectedOrder.Address = user.Address;
            selectedOrder.Email = user.Email;
            
            return selectedOrder;
        }

        public async Task<Object> GetAllOrders()
        {
            uow.OrderRepository.UpdateStatus();
            await uow.SaveAsync();
            var orders = await uow.OrderRepository.GetAllOrders();

            var ordersDtos = mapper.Map<IEnumerable<OrderDto>>(orders);
            List<SendOrderDto> allOrders = new List<SendOrderDto>();

            foreach (var orderDto in ordersDtos)
            {
                SendOrderDto order = new SendOrderDto();
                order.Id = orderDto.Id;
                order.Price = orderDto.Price;
                order.UserId = orderDto.UserId;
                order.Comment = orderDto.Comment;
                order.OrderStatus = orderDto.OrderStatus;
                order.Content = await uow.OrderRepository.GetOrderItems(orderDto.Id);
                var user = await uow.AccountRepository.GetUserDetails(orderDto.UserId);
                order.Address = user.Address;
                order.Email = user.Email;
                allOrders.Add(order);
            }
            return allOrders;
        }
        public async Task<Object> GetExecutedOrder(long id)
        {
            ApiError apiError = new ApiError();
            uow.OrderRepository.UpdateStatus();
            await uow.SaveAsync();
            var userRole = await uow.AccountRepository.GetUserDetails(id);

            if (userRole == null)
            {
                apiError.ErrorCode = 400;
                apiError.ErrorMessage = "Wrong user id";
                return apiError;
            }

            var orders = await uow.OrderRepository.GetExecutedOrders(id, userRole.Role.ToString());

            var ordersDtos = mapper.Map<IEnumerable<OrderDto>>(orders);
            List<SendOrderDto> allOrders = new List<SendOrderDto>();

            foreach (var orderDto in ordersDtos)
            {
                SendOrderDto order = new SendOrderDto();
                order.Id = orderDto.Id;
                order.Price = orderDto.Price;
                order.UserId = orderDto.UserId;
                order.Comment = orderDto.Comment;
                order.OrderStatus = orderDto.OrderStatus;
                order.Content = await uow.OrderRepository.GetOrderItems(orderDto.Id);
                var user = await uow.AccountRepository.GetUserDetails(orderDto.UserId);
                order.Address = user.Address;
                order.Email = user.Email;
                allOrders.Add(order);
            }
            return allOrders;
        }
        public async Task<Object> AcceptOrder(AcceptDeliveryDto deliverer, long id)
        {
            ApiError apiError = new ApiError();

            var deliveryGuy = await uow.AccountRepository.GetUserDetails(deliverer.Id);
            if (deliveryGuy == null)
            {
                apiError.ErrorCode = 400;
                apiError.ErrorMessage = "Wrong user id";
                return apiError;
            }

            if (await uow.OrderRepository.ChekIfDelivererIsVerifed(deliverer.Id))
            {
                apiError.ErrorCode = 400;
                apiError.ErrorMessage = "Deliverer is not verifed";
                return apiError;
            }

            if (await uow.OrderRepository.ChekIfDelivererIsWorking(deliverer.Id))
            {
                apiError.ErrorCode = 400;
                apiError.ErrorMessage = "Deliverer is allready working";
                return apiError;
            }
            var order = await uow.OrderRepository.GetOrder(id);

            if (order == null)
            {
                apiError.ErrorCode = 400;
                apiError.ErrorMessage = "Wrong order id";
                return apiError;
            }

            order.Deliverer = deliveryGuy.Id;
            order.OrderStatus = "Delivering";

            DateTime currentTime = DateTime.Now;
            DateTime currentTime30MinsLater = currentTime.AddMinutes(30);

            order.DeliveryTime = currentTime30MinsLater;

            await uow.SaveAsync();
            return 201;
        }
        public async Task<Object> CurrentOrders(long id)
        {
            ApiError apiError = new ApiError();
            uow.OrderRepository.UpdateStatus();
            await uow.SaveAsync();

            var theUser = await uow.AccountRepository.GetUserDetails(id);

            if (theUser == null)
            {
                apiError.ErrorCode = 400;
                apiError.ErrorMessage = "Wrong user id";
                return apiError;
            }

            var orders = await uow.OrderRepository.GetExecutingOrders(id, theUser.Role.ToString());

            var ordersDtos = mapper.Map<IEnumerable<CurrentOrderDto>>(orders);
            List<SendCurrentOrderDto> allOrders = new List<SendCurrentOrderDto>();

            foreach (var orderDto in ordersDtos)
            {
                SendCurrentOrderDto order = new SendCurrentOrderDto();
                order.Id = orderDto.Id;
                order.Price = orderDto.Price;
                order.UserId = orderDto.UserId;
                order.Comment = orderDto.Comment;
                order.OrderStatus = orderDto.OrderStatus;
                order.DeliveryTime = orderDto.DeliveryTime;
                order.Content = await uow.OrderRepository.GetOrderItems(orderDto.Id);
                var user = await uow.AccountRepository.GetUserDetails(orderDto.UserId);
                order.Address = user.Address;
                order.Email = user.Email;
                allOrders.Add(order);
            }
            return allOrders;
        }
    }
}
