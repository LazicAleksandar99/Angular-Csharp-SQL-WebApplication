using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Data.Repositorys
{
    public class ProductRepository : IProductRepository
    {
        private readonly DeliverySystemDbContext dsdc;

        public ProductRepository(DeliverySystemDbContext dsdc)
        {
            this.dsdc = dsdc;
        }

        public void AddProduct(Product product)
        {
            dsdc.Products.Add(product);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await dsdc.Products.ToListAsync();
        }
    }
}
