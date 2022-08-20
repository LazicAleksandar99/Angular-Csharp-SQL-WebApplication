using Backend.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface IProductService
    {
        Task<Object> GetProducts();
        Task<Object> AddProduct(NewProductDto newProduct);
    }
}
