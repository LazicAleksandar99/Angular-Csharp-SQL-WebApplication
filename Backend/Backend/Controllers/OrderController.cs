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

        [HttpPost("make/{id}")]
        public async Task<IActionResult> MakeOrder(MakeOrderDto[] order,long id)
        {
            Order newOrder = new Order();
            newOrder.CurrentOrdeer = true;
            newOrder.OrderAccepted = false;
            newOrder.SuccessfulDelivery = false;
            newOrder.UserId = id;

            uow.OrderRepository.AddOrder(newOrder);

            foreach(var item in order)
            {
                Item newItem = new Item();
                newItem.Quantity = item.Quantity;
                newItem.ProductId = item.Id;
                newItem.OrderId = newOrder.Id;
                uow.OrderRepository.AddItem(newItem);
                newOrder.OrderItems.Add(newItem);
            }

            //uow.OrderRepository.AddOrder()
           // long i = id;
            return StatusCode(201);
        }
    }
}
