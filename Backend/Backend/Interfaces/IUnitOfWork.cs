using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface IUnitOfWork
    {
        ILoginRepository LoginRepository { get; }

        Task<bool> SaveAsync();
    }
}
