using AutoMapper;
using Backend.Dtos;
using Backend.Errors;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IPhotoService photoService;

        public AccountController(IUnitOfWork uow, 
                                IConfiguration configuration, 
                                IMapper mapper,
                                IPhotoService photoService)
        {
            this.uow = uow;
            this.configuration = configuration;
            this.mapper = mapper;
            this.photoService = photoService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginReqDto loginReq)
        {
            var user = await uow.AccountRepository.Authenticate(loginReq.Email, loginReq.Password);

            ApiError apiError = new ApiError();

            if (user == null)
            {
                apiError.ErrorCode = Unauthorized().StatusCode;
                apiError.ErrorMessage = "Invalid user name or password";
                apiError.ErrorDetails = "This error appear when provided user id or password does not exists";
                return Unauthorized(apiError);
            }

            var loginRes = new LoginResDto();
            loginRes.Id = user.Id;
            loginRes.Token = CreateJWT(user);
            return Ok(loginRes);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationDto newAccount)
        {
            ApiError apiError = new ApiError();

            if (String.IsNullOrWhiteSpace(newAccount.Username) || String.IsNullOrWhiteSpace(newAccount.Password))
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "User name or password can not be blank";
                return BadRequest(apiError);
            }

            if (await uow.AccountRepository.UsernameAlreadyExists(newAccount.Username))
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "User already exists, please try different user name";
                return BadRequest(apiError);
            }

            if (String.IsNullOrWhiteSpace(newAccount.Email))
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Email can not be blank";
                return BadRequest(apiError);
            }

            if(await uow.AccountRepository.EmailAlreadyExists(newAccount.Email))
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Email already exists, please try different email";
                return BadRequest(apiError);
            }

            if (String.IsNullOrWhiteSpace(newAccount.Firstname) || newAccount.Firstname.Length < 3)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Firstname can not be blank or less then 3 characters";
                return BadRequest(apiError);
            }

            if (String.IsNullOrWhiteSpace(newAccount.Lastname) || newAccount.Lastname.Length < 3)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Lastname can not be blank or less then 3 characters";
                return BadRequest(apiError);
            }

            if(String.IsNullOrWhiteSpace(newAccount.Password) || newAccount.Password.Length < 8)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Password can not be blank or less then 8 characters";
                return BadRequest(apiError);
            }

            if(newAccount.Birthday < new DateTime(1900,1,1) || newAccount.Birthday > DateTime.Now.Date)//provjeriti u nekom jos dobu dali je uvijek tacno datum posto mozda ide po Londonu i onda sat vremena nije tacno..
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Persons birthday has to be between 1900.01.01 and current date";
                return BadRequest(apiError);
            }

            if (newAccount.Role == Userrole.Admin)
            {
                apiError.ErrorCode = 401;
                apiError.ErrorMessage = "You are not authorized for such action";
                return BadRequest(apiError);
            }

            if (String.IsNullOrWhiteSpace(newAccount.Address) || newAccount.Address.Length < 3)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Address can not be blank or less then 3 characters";
                return BadRequest(apiError);
            }

            uow.AccountRepository.Register(newAccount);
            await uow.SaveAsync();
            return StatusCode(201);
        }

        [HttpPost("update/{id}")]

        public async Task<IActionResult> Update(UserUpdateDto user, long id)
        {
            ApiError apiError = new ApiError();

            if (String.IsNullOrWhiteSpace(user.Username) || String.IsNullOrWhiteSpace(user.Username))
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "User name or password can not be blank";
                return BadRequest(apiError);
            }

            if(!await uow.AccountRepository.CheckUsername(id,user.Username))
            {
                if (await uow.AccountRepository.UsernameAlreadyExists(user.Username))
                {
                    apiError.ErrorCode = BadRequest().StatusCode;
                    apiError.ErrorMessage = "User already exists, please try different user name";
                    return BadRequest(apiError);
                }
            }

            if (String.IsNullOrWhiteSpace(user.Email))
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Email can not be blank";
                return BadRequest(apiError);
            }
            if(!await uow.AccountRepository.CheckEmail(id, user.Email))
            {
                if (await uow.AccountRepository.EmailAlreadyExists(user.Email))
                {
                    apiError.ErrorCode = BadRequest().StatusCode;
                    apiError.ErrorMessage = "Email already exists, please try different email";
                    return BadRequest(apiError);
                }
            }
            if (String.IsNullOrWhiteSpace(user.Firstname) || user.Firstname.Length < 3)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Firstname can not be blank or less then 3 characters";
                return BadRequest(apiError);
            }

            if (String.IsNullOrWhiteSpace(user.Lastname) || user.Lastname.Length < 3)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Lastname can not be blank or less then 3 characters";
                return BadRequest(apiError);
            }

            if (user.Birthday < new DateTime(1900, 1, 1) || user.Birthday > DateTime.Now.Date)//provjeriti u nekom jos dobu dali je uvijek tacno datum posto mozda ide po Londonu i onda sat vremena nije tacno..
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Persons birthday has to be between 1900.01.01 and current date";
                return BadRequest(apiError);
            }
            if (String.IsNullOrWhiteSpace(user.Address) || user.Address.Length < 3)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Address can not be blank or less then 3 characters";
                return BadRequest(apiError);
            }

            if(String.IsNullOrWhiteSpace(user.Oldpassword) && String.IsNullOrWhiteSpace(user.Newpassword))
            {
                uow.AccountRepository.Update(id, user);
            }
            else if((!String.IsNullOrWhiteSpace(user.Oldpassword) && !String.IsNullOrWhiteSpace(user.Newpassword)) &&
                    (user.Oldpassword.Length > 7 && user.Newpassword.Length > 7))
            {
                if (!await uow.AccountRepository.CheckPassword(id, user.Oldpassword))
                {
                    uow.AccountRepository.Update(id, user);
                }
            }
            else
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "Check you old and new password, they have to be both empty or both with values with length of minimum 8 characters";
                return BadRequest(apiError);
            }

            await uow.SaveAsync();
            return StatusCode(201);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetUserDetail(long id)
        {
            var user = await uow.AccountRepository.GetUserDetails(id);
            var userDetailsDto = mapper.Map<UserDetailsDto>(user);
            return Ok(userDetailsDto);
        }

        [HttpPost("photo/{id}")]
        public async Task<IActionResult> AddPhoto([FromForm(Name = "myfile")] IFormFile file, long id)
        {
            var result = await photoService.UploadPhotoAsync(file);
            if (result.Error != null)
                return BadRequest(result.Error.Message);

            //var user = await uow.AccountRepository.G
            uow.AccountRepository.UpdateUserPhoto(id, result.SecureUrl.AbsoluteUri);

            await uow.SaveAsync();
            return Ok(201); 
        }


        private string CreateJWT(User user)
        {
            var secretKey = configuration.GetSection("AppSettings:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(secretKey));

            var claims = new Claim[] {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Role,user.Role.ToString())
            };

            var signingCredentials = new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
