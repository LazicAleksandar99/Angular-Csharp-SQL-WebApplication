using AutoMapper;
using Backend.Dtos;
using Backend.Errors;
using Backend.Interfaces;
using Backend.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork uow;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IPhotoService photoService;

        public AccountService(IUnitOfWork uow,
                              IConfiguration configuration,
                              IMapper mapper,
                               IPhotoService photoService){
            this.uow = uow;
            this.configuration = configuration;
            this.mapper = mapper;
            this.photoService = photoService;
        }

        public async Task<Object> Login(LoginReqDto loginReq)
        {
            ApiError apiError = new ApiError();

            if (String.IsNullOrWhiteSpace(loginReq.Email))
            {
                apiError.ErrorCode = 400;//BadRequest().StatusCode;
                apiError.ErrorMessage = "Email can not be blank";
                return apiError;//BadRequest(apiError);
            }
            if(String.IsNullOrWhiteSpace(loginReq.Password) || loginReq.Password.Length < 8)
            {
                apiError.ErrorCode = 400;//BadRequest().StatusCode;
                apiError.ErrorMessage = "Password can not be blank or less then 8 characters";
                return apiError;//BadRequest(apiError);
            }

            var user = await uow.AccountRepository.Authenticate(loginReq.Email, loginReq.Password);


            if (user == null)
            {
                apiError.ErrorCode = 401;// Unauthorized().StatusCode;
                apiError.ErrorMessage = "Invalid user name or password";
                apiError.ErrorDetails = "This error appear when provided user id or password does not exists";
                return apiError; //Unauthorized(apiError);
            }

            var loginRes = new LoginResDto();
            loginRes.Id = user.Id;
            loginRes.Token = CreateJWT(user);
            return loginRes; //Ok(loginRes);
        }

        public async Task<Object> Register(RegistrationDto newAccount)
        {
            ApiError apiError = new ApiError();

            if (String.IsNullOrWhiteSpace(newAccount.Username) || String.IsNullOrWhiteSpace(newAccount.Password))
            {
                apiError.ErrorCode = 400;//BadRequest().StatusCode;
                apiError.ErrorMessage = "User name or password can not be blank";
                return apiError;
            }

            if (await uow.AccountRepository.UsernameAlreadyExists(newAccount.Username))
            {
                apiError.ErrorCode = 400;//BadRequest().StatusCode;
                apiError.ErrorMessage = "User already exists, please try different user name";
                return apiError;
            }

            if (String.IsNullOrWhiteSpace(newAccount.Email))
            {
                apiError.ErrorCode = 400;//BadRequest().StatusCode;
                apiError.ErrorMessage = "Email can not be blank";
                return apiError;
            }

            if (await uow.AccountRepository.EmailAlreadyExists(newAccount.Email))
            {
                apiError.ErrorCode = 400;//BadRequest().StatusCode;
                apiError.ErrorMessage = "Email already exists, please try different email";
                return apiError;
            }

            if (String.IsNullOrWhiteSpace(newAccount.Firstname) || newAccount.Firstname.Length < 3)
            {
                apiError.ErrorCode = 400;//BadRequest().StatusCode;
                apiError.ErrorMessage = "Firstname can not be blank or less then 3 characters";
                return apiError;
            }

            if (String.IsNullOrWhiteSpace(newAccount.Lastname) || newAccount.Lastname.Length < 3)
            {
                apiError.ErrorCode = 400;//BadRequest().StatusCode;
                apiError.ErrorMessage = "Lastname can not be blank or less then 3 characters";
                return apiError;
            }

            if (String.IsNullOrWhiteSpace(newAccount.Password) || newAccount.Password.Length < 8)
            {
                apiError.ErrorCode = 400;//BadRequest().StatusCode;
                apiError.ErrorMessage = "Password can not be blank or less then 8 characters";
                return apiError;
            }

            if (newAccount.Birthday < new DateTime(1900, 1, 1) || newAccount.Birthday > DateTime.Now.Date)//provjeriti u nekom jos dobu dali je uvijek tacno datum posto mozda ide po Londonu i onda sat vremena nije tacno..
            {
                apiError.ErrorCode = 400;//BadRequest().StatusCode;
                apiError.ErrorMessage = "Persons birthday has to be between 1900.01.01 and current date";
                return apiError;
            }
            if (newAccount.Role == Userrole.Admin)
            {
                apiError.ErrorCode = 401;
                apiError.ErrorMessage = "You are not authorized for such action";
                return apiError;
            }

            if (String.IsNullOrWhiteSpace(newAccount.Address) || newAccount.Address.Length < 3)
            {
                apiError.ErrorCode = 400;//BadRequest().StatusCode;
                apiError.ErrorMessage = "Address can not be blank or less then 3 characters";
                return apiError;
            }

            uow.AccountRepository.Register(newAccount);
            await uow.SaveAsync();
            var newUser = await uow.AccountRepository.GetUserDetailsByUsername(newAccount.Username);//je l' ovo potrebno..
            return newUser.Id;
        }

        public async Task<Object> Update(UserUpdateDto user, long id)
        {
            ApiError apiError = new ApiError();

            if (String.IsNullOrWhiteSpace(user.Username) || String.IsNullOrWhiteSpace(user.Username))
            {
                apiError.ErrorCode = 400;//BadRequest().StatusCode;
                apiError.ErrorMessage = "User name or password can not be blank";
                return apiError;
            }

            if (!await uow.AccountRepository.CheckUsername(id, user.Username))
            {
                if (await uow.AccountRepository.UsernameAlreadyExists(user.Username))
                {
                    apiError.ErrorCode = 400;//BadRequest().StatusCode;
                    apiError.ErrorMessage = "User already exists, please try different user name";
                    return apiError;
                }
            }

            if (String.IsNullOrWhiteSpace(user.Email))
            {
                apiError.ErrorCode = 400;//BadRequest().StatusCode;
                apiError.ErrorMessage = "Email can not be blank";
                return apiError;
            }
            if (!await uow.AccountRepository.CheckEmail(id, user.Email))
            {
                if (await uow.AccountRepository.EmailAlreadyExists(user.Email))
                {
                    apiError.ErrorCode = 400;//BadRequest().StatusCode;
                    apiError.ErrorMessage = "Email already exists, please try different email";
                    return apiError;
                }
            }
            if (String.IsNullOrWhiteSpace(user.Firstname) || user.Firstname.Length < 3)
            {
                apiError.ErrorCode = 400;//BadRequest().StatusCode;
                apiError.ErrorMessage = "Firstname can not be blank or less then 3 characters";
                return apiError;
            }

            if (String.IsNullOrWhiteSpace(user.Lastname) || user.Lastname.Length < 3)
            {
                apiError.ErrorCode = 400;//BadRequest().StatusCode;
                apiError.ErrorMessage = "Lastname can not be blank or less then 3 characters";
                return apiError;
            }

            if (user.Birthday < new DateTime(1900, 1, 1) || user.Birthday > DateTime.Now.Date)
            {
                apiError.ErrorCode = 400;//BadRequest().StatusCode;
                apiError.ErrorMessage = "Persons birthday has to be between 1900.01.01 and current date";
                return apiError;
            }
            if (String.IsNullOrWhiteSpace(user.Address) || user.Address.Length < 3)
            {
                apiError.ErrorCode = 400;//BadRequest().StatusCode;
                apiError.ErrorMessage = "Address can not be blank or less then 3 characters";
                return apiError ;
            }

            if (String.IsNullOrWhiteSpace(user.Oldpassword) && String.IsNullOrWhiteSpace(user.Newpassword))
            {
                uow.AccountRepository.Update(id, user);
            }
            else if ((!String.IsNullOrWhiteSpace(user.Oldpassword) && !String.IsNullOrWhiteSpace(user.Newpassword)) &&
                    (user.Oldpassword.Length > 7 && user.Newpassword.Length > 7))
            {
                if (!await uow.AccountRepository.CheckPassword(id, user.Oldpassword))
                {
                    uow.AccountRepository.Update(id, user);
                }
            }
            else
            {
                apiError.ErrorCode = 400;//BadRequest().StatusCode;
                apiError.ErrorMessage = "Check you old and new password, they have to be both empty or both with values with length of minimum 8 characters";
                return apiError;
            }

            await uow.SaveAsync();
            return 201;//else
        }
        
        public async Task<Object> GetUserDetail(long id)
        {
            ApiError apiError = new ApiError();
            var user = await uow.AccountRepository.GetUserDetails(id);

            if (user == null)
            {
                apiError.ErrorCode = 400;
                apiError.ErrorMessage = "Wrong user id";
                return apiError;
            }

            var userDetailsDto = mapper.Map<UserDetailsDto>(user);
            return userDetailsDto;
        }

        public async Task<Object> AddPhoto(IFormFile file, long id)
        {
            var result = await photoService.UploadPhotoAsync(file);
            if (result.Error != null)
                return result.Error.Message;

            uow.AccountRepository.UpdateUserPhoto(id, result.SecureUrl.AbsoluteUri);

            await uow.SaveAsync();
            return 201;
        }
        public async Task<Object> GetDelivers()
        {
            var delivers = await uow.AccountRepository.GetAllDelivers();

            var deliversDto = mapper.Map<IEnumerable<DeliverDto>>(delivers);

            return deliversDto;
        }

        public async Task<Object> Verify(VrifyDto user)
        {
            ApiError apiError = new ApiError();
            var deliverer = await uow.AccountRepository.GetUserDetailsByUsername(user.Username);
            if (deliverer.Role.ToString() != "Deliverer")
            {
                apiError.ErrorCode = 400;
                apiError.ErrorMessage = "Trying to verify someone that is not Deliverer";
                return apiError;
            }
            if (String.IsNullOrWhiteSpace(user.Username))
            {
                apiError.ErrorCode = 400;
                apiError.ErrorMessage = "You have to send username of the person you want to verify";
                return apiError;
            }

            uow.AccountRepository.Verify(user.Username);
            string text = "Congretulations! Your account is verified, you can do your first delivery job TODAY!";
            sendEmail(deliverer.Username, deliverer.Email, text);
            await uow.SaveAsync();
            return 201;
        }

        public async Task<Object> Deny(VrifyDto user)
        {
            ApiError apiError = new ApiError();
            var deliverer = await uow.AccountRepository.GetUserDetailsByUsername(user.Username);
            if (deliverer.Role.ToString() != "Deliverer")
            {
                apiError.ErrorCode = 400;
                apiError.ErrorMessage = "Trying to verify someone that is not Deliverer";
                return apiError;
            }
            if (String.IsNullOrWhiteSpace(user.Username))
            {
                apiError.ErrorCode = 400;
                apiError.ErrorMessage = "You have to send username of the person you want to verify";
                return apiError;
            }

            uow.AccountRepository.Deny(user.Username);
            string text = "Unfortunately you are not accepted to our company. We are grateful for you application but currently we dont need new employes";
            sendEmail(deliverer.Username, deliverer.Email, text);
            await uow.SaveAsync();
            return 201;
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
