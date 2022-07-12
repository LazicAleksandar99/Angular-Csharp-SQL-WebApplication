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
        Task<bool> CheckPassword(long id, string oldpassword);
        Task<bool> CheckUsername(long id, string username);
        Task<bool> CheckEmail(long id, string email);
        void Update(long id,UserUpdateDto userUpdate);
    }
}
