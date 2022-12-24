using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quotes.Base;
using Quotes.DTO.Requests;
using Quotes.Repositories.Interfaces;

namespace Quotes.Controllers
{
    // [Route("api/[controller]")]
    [Route("api/user")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserRepository repository;

        public UserController(IUserRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost("login")]
        public IActionResult Login([FromForm] LoginRequest request)
        {
            return ReturnActionResult(repository.Login(request));
        }

        [HttpPost("register")]
        public IActionResult Register([FromForm] RegisterRequest request)
        {
            return ReturnActionResult(repository.Register(request));
        }

    }
}
