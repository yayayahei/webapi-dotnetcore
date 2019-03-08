using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hello.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AuthorizedController : ControllerBase
    {
        public bool Get()
        {
            return true;
        }
    }
}