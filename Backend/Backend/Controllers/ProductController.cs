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
    public class ProductController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IProductService productService;

        public ProductController(IUnitOfWork uow, IMapper mapper, IProductService productService)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.productService = productService;
        }

        //Sta ako nema jos uvijek ni jedan produkt?
        [HttpGet("list")]
        [Authorize(Roles = "Admin,NormalUser")]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(await productService.GetProducts());
        }

        //gotova provjera
        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(NewProductDto newProduct)
        {
            var result = await productService.AddProduct(newProduct);

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
    }
}
