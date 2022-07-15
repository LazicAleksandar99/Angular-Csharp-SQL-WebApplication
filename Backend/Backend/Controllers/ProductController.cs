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

        public ProductController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        //gotova provjera
        [HttpGet("list")]
        [Authorize(Roles = "Admin,NormalUser")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await uow.ProductRepository.GetProductsAsync();

            var productsDto = mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productsDto);
        }

        //gotova provjera
        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(NewProductDto newProduct)
        {
            ApiError apiError = new ApiError();

            if (String.IsNullOrWhiteSpace(newProduct.Name) || newProduct.Name.Length < 2)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Product name not valid";
                return BadRequest(apiError);
            }

            if (newProduct.Price < 0 || newProduct.Price > 1000000)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Price not valid";
                return BadRequest(apiError);
            }

            if (String.IsNullOrWhiteSpace(newProduct.Ingredients) || newProduct.Ingredients.Length < 2)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Ingredients name not valid";
                return BadRequest(apiError);
            }

            var newProd = mapper.Map<Product>(newProduct);

            uow.ProductRepository.AddProduct(newProd);
            await uow.SaveAsync();
            return Ok(201);
        }
    }
}
