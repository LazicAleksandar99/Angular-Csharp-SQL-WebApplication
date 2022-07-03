using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface ILoginRepository
    {
        Task<Person> Authenticate(string userName, string password);

    }
}
