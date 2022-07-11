using Backend.Dtos;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface IAccountRepository
    {
        Task<User> Authenticate(string email, string password);
        void Register(RegistrationDto newAccount);
        Task<bool> UsernameAlreadyExists(string userName);
        Task<bool> EmailAlreadyExists(string email);
        Task<User> GetUserDetails(long id);
        void UpdateUserPhoto(long id, string photo);
        
    }
}
