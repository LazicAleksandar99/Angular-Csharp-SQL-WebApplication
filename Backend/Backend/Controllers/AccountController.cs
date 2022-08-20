using AutoMapper;
using Backend.Dtos;
using Backend.Errors;
using Backend.Interfaces;
using Backend.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Controllers
{   
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IPhotoService photoService;
        private readonly IAccountService accountService;

        public AccountController(IUnitOfWork uow, 
                                IConfiguration configuration, 
                                IMapper mapper,
                                IPhotoService photoService,
                                IAccountService accountService)
        {
            this.uow = uow;
            this.configuration = configuration;
            this.mapper = mapper;
            this.photoService = photoService;
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
        private void sendEmail(string username, string userEmail,string text)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Verification Confirmation", "vasilije240799@gmail.com"));
            message.To.Add(new MailboxAddress(username, "ipleydota2@gmail.com"));//ovdje bi trebalo userEmail, ali da se ne salje svaki + plus nevalidne cu praviti emailove tako da bolje ovako
            message.Subject = "Verification";
            message.Body = new TextPart("plain")
            {
                Text = text 
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 465, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate("vasilije240799@gmail.com", "hijipitqgequumcb");
                    client.Send(message);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
