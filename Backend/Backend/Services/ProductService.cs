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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public ProductService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<Object> GetProducts()
        {
            var products = await uow.ProductRepository.GetProductsAsync();
            var productsDto = mapper.Map<IEnumerable<ProductDto>>(products);
            return productsDto;
        }
        public async Task<Object> AddProduct(NewProductDto newProduct)
        {
            ApiError apiError = new ApiError();

            if (String.IsNullOrWhiteSpace(newProduct.Name) || newProduct.Name.Length < 2)
            {
                apiError.ErrorCode = 400;
                apiError.ErrorMessage = "Product name not valid";
                return apiError;
            }

            if (newProduct.Price < 0 || newProduct.Price > 1000000)
            {
                apiError.ErrorCode = 400;
                apiError.ErrorMessage = "Price not valid";
                return apiError;
            }

            if (String.IsNullOrWhiteSpace(newProduct.Ingredients) || newProduct.Ingredients.Length < 2)
            {
                apiError.ErrorCode = 400;
                apiError.ErrorMessage = "Ingredients name not valid";
                return apiError;
            }

            var newProd = mapper.Map<Product>(newProduct);

            uow.ProductRepository.AddProduct(newProd);
            await uow.SaveAsync();
            return 201;
        }
    }
}
