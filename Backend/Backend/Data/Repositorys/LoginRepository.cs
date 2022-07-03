using Backend.Helpers;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Repositorys
{
    public class LoginRepository : ILoginRepository
    {
        private readonly DeliverySystemDbContext dsdc;
        private readonly AuthenticationHelper autHelper;

        public LoginRepository(DeliverySystemDbContext dsdc)
        {
            this.dsdc = dsdc;
        }

        public async Task<Person> Authenticate(string userName, string passwordText)
        {
            var user = await dsdc.Users.FirstOrDefaultAsync(x => x.Username == userName);

            if (user == null || user.PasswordKey == null)
                return null;

            if (!autHelper.MatchPasswordHash(passwordText, user.Password, user.PasswordKey))
                return null;

            return user;
        }
    }
}
