using AutoMapper;
using Backend.Dtos;
using Backend.Errors;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public OrderController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        //u foreach jos neke provjere
        [HttpPost("make/{id}/{comment}")]
        [Authorize(Roles = "NormalUser")]
        public async Task<IActionResult> MakeOrder(MakeOrderDto[] order, long id, string comment)
        {
            ApiError apiError = new ApiError();
            var user = await uow.AccountRepository.GetUserDetails(id);
            if (user == null)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Wrong user id";
                return BadRequest(apiError);
            }
            if (String.IsNullOrWhiteSpace(comment))
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "You need to enter comment";
                return BadRequest(apiError);
            }

            float grandTotal = 0;
            Order newOrder = new Order();
            newOrder.OrderStatus = "Pending";
            newOrder.Deliverer = -1;
            newOrder.Comment = comment;
            newOrder.UserId = id;

            uow.OrderRepository.AddOrder(newOrder);
            await uow.SaveAsync();

            foreach (var item in order)
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
            return StatusCode(201);
        }

        //gotova provjera
        [HttpGet("pending")]
        [Authorize(Roles = "Deliverer")]
        public async Task<IActionResult> GetPendingOrders()
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
            return Ok(pendingOrders);
        }

        //gotova provjera
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
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
            return Ok(allOrders);
        }

        //gotova provjera
        [HttpGet("executed/{id}")]
        [Authorize(Roles = "Deliverer,NormalUser")]
        public async Task<IActionResult> GetExecutedOrder(long id)
        {
            ApiError apiError = new ApiError();
            uow.OrderRepository.UpdateStatus();
            await uow.SaveAsync();
            var userRole = await uow.AccountRepository.GetUserDetails(id);

            if (userRole == null)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Wrong user id";
                return BadRequest(apiError);
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
            return Ok(allOrders);
        }

        //gotova provjera
        [HttpPost("accept/{id}")]
        [Authorize(Roles = "Deliverer")]
        public async Task<IActionResult> AcceptOrder(AcceptDeliveryDto deliverer, long id)
        {
            ApiError apiError = new ApiError();

            var deliveryGuy = await uow.AccountRepository.GetUserDetails(deliverer.Id);
            if (deliveryGuy == null)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Wrong user id";
                return BadRequest(apiError);
            }

            if (await uow.OrderRepository.ChekIfDelivererIsVerifed(deliverer.Id))
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Deliverer is not verifed";
                return BadRequest(apiError);
            }

            if (await uow.OrderRepository.ChekIfDelivererIsWorking(deliverer.Id))
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Deliverer is allready working";
                return BadRequest(apiError);
            }
            var order = await uow.OrderRepository.GetOrder(id);

            if(order == null)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Wrong order id";
                return BadRequest(apiError);
            }

            order.Deliverer = deliveryGuy.Id;
            order.OrderStatus = "Delivering";

            DateTime currentTime = DateTime.Now;
            DateTime x30MinsLater = currentTime.AddMinutes(30);

            order.DeliveryTime = x30MinsLater;

            await uow.SaveAsync();
            return Ok(201);
        }

        //gotova provjera
        [HttpGet("current/{id}")]
        [Authorize(Roles = "Deliverer,NormalUser")]
        public async Task<IActionResult> CurrentOrders(long id)
        {
            ApiError apiError = new ApiError();
            uow.OrderRepository.UpdateStatus();
            await uow.SaveAsync();

            var userRole = await uow.AccountRepository.GetUserDetails(id);

            if(userRole == null)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Wrong user id";
                return BadRequest(apiError);
            }

            var orders = await uow.OrderRepository.GetExecutingOrders(id,userRole.Role.ToString());

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
            return Ok(allOrders);
        }
    }
}
