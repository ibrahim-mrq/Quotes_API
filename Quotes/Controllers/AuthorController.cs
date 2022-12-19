using Microsoft.AspNetCore.Mvc;
using Quotes.Base;
using Quotes.DTO.Requests;
using Quotes.Repositories.Interfaces;

namespace Quotes.Controllers
{
    // [Route("api/[controller]")]
    [Route("api/author")]
    [ApiController]
    public class AuthorController : BaseController
    {
        private readonly IAuthorRepository repository;

        public AuthorController(IAuthorRepository repository)
        {
            this.repository = repository;
        }


        [HttpPost("add_author")]
        public IActionResult Add([FromForm] AddAuthorRequest request)
        {
            return ReturnActionResult(repository.Add(request));
        }

        [HttpPut("update_author/{AutherId}")]
        public IActionResult Update([FromRoute] int AutherId, [FromForm] AddAuthorRequest request)
        {
            return ReturnActionResult(repository.Update(AutherId, request));
        }

        [HttpDelete("remove_author/{AutherId}")]
        public IActionResult Delete([FromRoute] int AutherId)
        {
            return ReturnActionResult(repository.Delete(AutherId));
        }


        [HttpGet("get_all_authers")]
        public IActionResult GetAll()
        {
            return ReturnActionResult(repository.GetAll());
        }

        [HttpGet("get_author_by_id")]
        public IActionResult GetQuoteById([FromQuery(Name = "auther_id")] int AutherId)
        {
            return ReturnActionResult(repository.GetById(AutherId));
        }


        [HttpDelete("clear_authers")]
        public IActionResult Clear()
        {
            return ReturnActionResult(repository.Clear());
        }


    }
}
