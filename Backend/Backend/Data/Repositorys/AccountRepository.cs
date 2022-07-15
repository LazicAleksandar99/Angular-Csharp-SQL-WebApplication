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

            if (newAccount.Role == Userrole.NormalUser)
            {
                user.Role = Userrole.NormalUser;
                user.Verification = "";
            }
            else
            {
                user.Role = Userrole.Deliverer;
                user.Verification = "Pending";
            }

            dsdc.Users.Add(user);
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

           // dsdc.SaveChanges();
        }

        public async Task<bool> CheckPassword(long id, string oldpassword)
        {
            byte[] oldPasswordHash, passwordKey;
            bool ret = false;

            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                oldPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(oldpassword));

            }
            User user = await dsdc.Users.Where(u => u.Id == id).FirstAsync();
            
            if (user.Password == oldPasswordHash)
                ret = true;

            return ret;
        }


        public async Task<bool> CheckUsername(long id, string username)
        {
            bool ret = false;
            User user = await dsdc.Users.Where(u => u.Id == id).FirstAsync();

            if (user.Username == username)
                ret = true;
            return ret;
        }
        public async Task<bool> CheckEmail(long id, string email)
        {
            bool ret = false;
            User user = await dsdc.Users.Where(u => u.Id == id).FirstAsync();

            if (user.Email == email)
                ret = true;
            return ret;
        }

        public void Update(long id,UserUpdateDto userUpdate)
        {
            var user = dsdc.Users.SingleOrDefault(x => x.Id == id);

            byte[] passwordHash, passwordKey;

            using (var hmac = new HMACSHA512())
            {
                if (!String.IsNullOrWhiteSpace(userUpdate.Newpassword))
                {
                    passwordKey = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userUpdate.Newpassword));
                    user.Password = passwordHash;
                    user.PasswordKey = passwordKey;
                }
            }
            
            user.Address = userUpdate.Address;
            user.Birthday = userUpdate.Birthday;
            user.Email = userUpdate.Email;
            user.Firstname = userUpdate.Firstname;
            user.Lastname = userUpdate.Lastname;
            user.Username = userUpdate.Username;
        }

        public async Task<IEnumerable<User>> GetAllDelivers()
        {
            return await dsdc.Users.Where(x => x.Verification != "").ToListAsync();
        }
        public void Verify(string username)
        {
            var user = dsdc.Users.SingleOrDefault(x => x.Username == username);
            user.Verification = "Verified";
        }
        public void Deny(string username)
        {
            var user = dsdc.Users.SingleOrDefault(x => x.Username == username);
            user.Verification = "Denied";
        }

        public async Task<User> GetUserDetailsByUsername(string username)
        {
            User user = await dsdc.Users.Where(u => u.Username == username).FirstAsync();
            return user;
        }
    }
}
