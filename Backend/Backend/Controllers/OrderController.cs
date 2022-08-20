using Backend.Dtos;
using Backend.Errors;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        //u foreach jos neke provjere
        [HttpPost("make/{id}/{comment}")]
        [Authorize(Roles = "NormalUser")]
        public async Task<IActionResult> MakeOrder(MakeOrderDto[] order, long id, string comment)
        {
            var result = await orderService.MakeOrder(order, id, comment);

            if (result is ApiError)
            {
                ApiError apiError = (ApiError)result;
                return BadRequest(apiError);
            }
            else
            {
                return StatusCode(201);
            }
        }

        //Sta ako je prazano?
        [HttpGet("pending")]
        [Authorize(Roles = "Deliverer")]
        public async Task<IActionResult> GetPendingOrders()
        {
            return Ok(await orderService.GetPendingOrders());
        }

        //Sta ako je prazano?
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            return Ok(await orderService.GetAllOrders());
        }

        //Sta ako je prazano?
        [HttpGet("executed/{id}")]
        [Authorize(Roles = "Deliverer,NormalUser")]
        public async Task<IActionResult> GetExecutedOrder(long id)
        {
            var result = await orderService.GetExecutedOrder(id);

            if (result is ApiError)
            {
                ApiError apiError = (ApiError)result;
                return BadRequest(apiError);
            }
            else
            {
                return Ok(result);
            }
        }
        
        [HttpPost("accept/{id}")]
        [Authorize(Roles = "Deliverer")]
        public async Task<IActionResult> AcceptOrder(AcceptDeliveryDto deliverer, long id)
        {
            var result = await orderService.AcceptOrder(deliverer, id);

            if (result is ApiError)
            {
                ApiError apiError = (ApiError)result;
                return BadRequest(apiError);
            }
            else
            {
                return StatusCode(201);
            }
        }

        //gotova provjera
        [HttpGet("current/{id}")]
        [Authorize(Roles = "Deliverer,NormalUser")]
        public async Task<IActionResult> CurrentOrders(long id)
        {
            var result = await orderService.CurrentOrders(id);

            if (result is ApiError)
            {
                ApiError apiError = (ApiError)result;
                return BadRequest(apiError);
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
