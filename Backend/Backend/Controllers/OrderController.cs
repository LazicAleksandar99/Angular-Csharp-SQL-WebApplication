using AutoMapper;
using Backend.Dtos;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public OrderController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        [HttpPost("make/{id}/{comment}")]
        public async Task<IActionResult> MakeOrder(MakeOrderDto[] order,long id,string comment)
        {
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

        [HttpGet("orders")]
        public async Task<IActionResult> GetPendingOrders()
        {
            var orders = await uow.OrderRepository.GetPendingOrders();

            var ordersDtos = mapper.Map<IEnumerable<PendingOrderDto>>(orders);
            List<SendPendingOrderDto> pendingOrders = new List<SendPendingOrderDto>();

            foreach(var orderDto in ordersDtos)
            {
                SendPendingOrderDto order = new SendPendingOrderDto();
                order.Id = orderDto.Id;
                order.Price = orderDto.Price;
                order.UserId = orderDto.UserId;
                order.Comment = orderDto.Comment;
                var user = await uow.AccountRepository.GetUserDetails(orderDto.UserId);
                order.Address = user.Address;
                order.Email = user.Email;
                pendingOrders.Add(order);
            }
            return Ok(pendingOrders);
        }
    }
}
