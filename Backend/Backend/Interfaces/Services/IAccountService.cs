using Backend.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface IAccountService
    {
        Task<Object> Login(LoginReqDto loginReq);
        Task<Object> Register(RegistrationDto newAccount);
        Task<Object> Update(UserUpdateDto user, long id);
        Task<Object> GetUserDetail(long id);
        Task<Object> AddPhoto(IFormFile file, long id);
        Task<Object> GetDelivers();
        Task<Object> Verify(VrifyDto user);
        Task<Object> Deny(VrifyDto user);
    }
}
