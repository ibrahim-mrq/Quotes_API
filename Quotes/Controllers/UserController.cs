using Microsoft.AspNetCore.Mvc;
using Quotes.Authorize;
using Quotes.Base;
using Quotes.DTO.Requests.User;
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

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromForm] LoginRequest request)
        {
            return ReturnActionResult(repository.Login(request));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromForm] RegisterRequest request)
        {
            return ReturnActionResult(repository.Register(request));
        }

        [AllowAnonymous]
        [HttpPost("forgot_password")]
        public IActionResult ForgotPassword([FromForm] ForgotPasswordRequest request)
        {
            return ReturnActionResult(repository.ForgotPassword(request));
        }

        [AllowAnonymous]
        [HttpPost("reset_password")]
        public IActionResult ResetPassword([FromForm] ResetPasswordRequest request)
        {
            return ReturnActionResult(repository.ResetPassword(request));
        }

        [HttpPut("update")]
        public IActionResult Update([FromForm] UpdateUserRequest request)
        {
            return ReturnActionResult(repository.Update(request));
        }

        [HttpDelete("delete/{Id}")]
        public IActionResult Delete([FromRoute] int Id)
        {
            return ReturnActionResult(repository.Delete(Id));
        }

        [HttpPost("retrieve/{Id}")]
        public IActionResult Retrieve([FromRoute] int Id)
        {
            return ReturnActionResult(repository.Retrieve(Id));
        }


        [HttpGet("get_all_users")]
        public IActionResult GetAll()
        {
            return ReturnActionResult(repository.GetAll());
        }

        [HttpGet("get_user")]
        public IActionResult GetUser([FromQuery(Name = "id")] int Id)
        {
            var userId = GetUserId();
            return ReturnActionResult(repository.GetUser(userId));
        }


        [HttpDelete("clear_users")]
        public IActionResult ClearQuotes()
        {
            return ReturnActionResult(repository.Clear());
        }

    }
}
