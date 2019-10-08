using System;
using System.Threading.Tasks;
using Hello.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hello.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly HelloDbContext _dbContext;

        public UsersController(HelloDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("{name}")]
        public async Task<User> Get(string name)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(user => user.Name == name);
        }
    }
}