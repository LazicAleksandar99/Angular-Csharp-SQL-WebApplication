using Backend.Dtos;
using Backend.Helpers;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Backend.Data.Repositorys
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DeliverySystemDbContext dsdc;
        private readonly AuthenticationHelper autHelper;

        public AccountRepository(DeliverySystemDbContext dsdc)
        {
            this.dsdc = dsdc;
            autHelper = new AuthenticationHelper();
        }

        public async Task<User> Authenticate(string email, string passwordText)
        {
            var user = await dsdc.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null || user.PasswordKey == null)
                return null;

            if (!autHelper.MatchPasswordHash(passwordText, user.Password, user.PasswordKey))
                return null;

            return user;
        }

        public void Register(RegistrationDto newAccount)
        {
            byte[] passwordHash, passwordKey;
            

            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(newAccount.Password));

            }

            if(newAccount.Role == Userrole.NormalUser)
            {
                User user = new User();
                user.Username = newAccount.Username;
                user.Password = passwordHash;
                user.PasswordKey = passwordKey;
                user.Address = newAccount.Address;
                user.Birthday = newAccount.Birthday;
                user.Email = newAccount.Email;
                user.Firstname = newAccount.Firstname;
                user.Lastname = newAccount.Lastname;
                user.Role = Userrole.NormalUser;

                dsdc.Users.Add(user);
            }
            else
            {
                //Deliverer user = new Deliverer();
                //user.Username = newAccount.Username;
                //user.Password = passwordHash;
                //user.PasswordKey = passwordKey;
                //user.Address = newAccount.Address;
                //user.Birthday = newAccount.Birthday;
                //user.Email = newAccount.Email;
                //user.Name = newAccount.Name;
                //user.Lastname = newAccount.Lastname;
                //user.Role = Userrole.NormalUser;

               // dsdc.Deliverers.Add(user);
            }
        }

        public async Task<bool> UsernameAlreadyExists(string userName)
        {
            return await dsdc.Users.AnyAsync(x => x.Username == userName);
        }

        public async Task<bool> EmailAlreadyExists(string email)
        {
            return await dsdc.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<User> GetUserDetails(long id)
        {
            User user = await dsdc.Users.Where(u => u.Id == id).FirstAsync();
            return user;
        }

        public void UpdateUserPhoto(long id, string photo)
        {
            var user = dsdc.Users.SingleOrDefault(x => x.Id == id);
            user.Picture = photo;

            dsdc.SaveChanges();
        }
    }
}
