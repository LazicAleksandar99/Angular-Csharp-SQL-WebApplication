using Backend.Dtos;
using Backend.Errors;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{   
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginReqDto loginReq)
        {
            var result = await accountService.Login(loginReq);
            
            if(result is ApiError)
            {
                ApiError apiError = (ApiError)result;
                if (apiError.ErrorCode == 400)
                    return BadRequest(apiError);
                else
                    return Unauthorized(apiError);
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPost("register")]  
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegistrationDto newAccount)
        {
            var result = await accountService.Register(newAccount);

            if (result is ApiError)
            {
                ApiError apiError = (ApiError)result;
                if (apiError.ErrorCode == 400)
                    return BadRequest(apiError);
                else
                    return Unauthorized(apiError);
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPost("update/{id}")]
        public async Task<IActionResult> Update(UserUpdateDto user, long id)
        {
            var result = await accountService.Update(user, id);

            if (result is ApiError)
            {
                ApiError apiError = (ApiError)result;
                return BadRequest(apiError);
            }
            else
            {
                return StatusCode(201);
            }
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetUserDetail(long id)
        {
            var result = await accountService.GetUserDetail(id);

            if (result is ApiError)
            {
                ApiError apiError = (ApiError)result;
                return BadRequest(apiError);
            }
            else
            {
                return Ok(result);
            }
        }

        //ovo se mora provjeriti pod HITNO!
        [HttpPost("photo/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> AddPhoto([FromForm(Name = "myfile")] IFormFile file, long id)
        {
            var result = await accountService.AddPhoto(file, id);

            if (result is string)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(201);
            }
        }

        //Sta kad nema nijedan deliveri, to moram vidjet
        [HttpGet("delivers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDelivers()
        {
            return Ok(await accountService.GetDelivers());
        }

        //Sta ako ne postoji user s tim usernamom?
        [HttpPost("verify")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Verify(VrifyDto user)
        {
            var result = await accountService.Verify(user);

            if (result is ApiError)
            {
                ApiError apiError = (ApiError)result;
                return BadRequest(apiError);
            }
            else
            {
                return StatusCode(201);
            }
        }

        //Sta ako ne postoji user s tim usernamom?
        [HttpPost("deny")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deny(VrifyDto user)
        {
            var result = await accountService.Deny(user);

            if (result is ApiError)
            {
                ApiError apiError = (ApiError)result;
                return BadRequest(apiError);
            }
            else
            {
                return StatusCode(201);
            }
        }
    }
}
