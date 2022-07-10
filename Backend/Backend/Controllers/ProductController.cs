using AutoMapper;
using Backend.Dtos;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public ProductController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        [HttpGet("list")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts()
        {
            var products = await uow.ProductRepository.GetProductsAsync();

            var productsDto = mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productsDto);
        }
    }
}
