using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hello.Models;
using Hello.Utils;
using Microsoft.Extensions.Configuration;

namespace Hello.Repositories
{
    public class UserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<User> GetUser(string name, string password)
        {
            List<User> users = new List<User>();
            _configuration.GetSection("Users").Bind(users);
            var result = users.FirstOrDefault(user =>
                name.Equals(user.Name, StringComparison.CurrentCultureIgnoreCase) &&
                MD5Helper.GetMd5Hash(password).Equals(user.Password));
            return result;
        }
    }
}