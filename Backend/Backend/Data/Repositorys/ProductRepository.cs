using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Repositorys
{
    public class ProductRepository : IProductRepository
    {
        private readonly DeliverySystemDbContext dsdb;

        public ProductRepository(DeliverySystemDbContext dsdb)
        {
            this.dsdb = dsdb;
        }

        public void AddProduct(Product product)
        {
            dsdb.Products.Add(product);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await dsdb.Products.ToListAsync();
        }
    }
}
