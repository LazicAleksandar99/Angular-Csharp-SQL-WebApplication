using Backend.Data.Repositorys;
using Backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DeliverySystemDbContext dsdc;

        public UnitOfWork(DeliverySystemDbContext dsdc)
        {
            this.dsdc = dsdc;
        }

        public IAccountRepository AccountRepository => new AccountRepository(dsdc);

        public IProductRepository ProductRepository => new ProductRepository(dsdc);

        public async Task<bool> SaveAsync()
        {
            return await dsdc.SaveChangesAsync() > 0;
        }
    }
}
