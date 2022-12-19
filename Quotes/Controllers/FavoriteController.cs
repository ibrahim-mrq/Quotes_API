using Microsoft.AspNetCore.Mvc;
using Quotes.Base;
using Quotes.DTO.Requests;
using Quotes.Repositories.Interfaces;

namespace Quotes.Controllers
{
    //  [Route("api/[controller]")]
    [Route("api/favorite")]
    [ApiController]
    public class FavoriteController : BaseController
    {

        private readonly IFavoriteRepository repository;

        public FavoriteController(IFavoriteRepository repository)
        {
            this.repository = repository;
        }


        [HttpPost("add_favorite")]
        public IActionResult Add([FromForm] AddFavoriteRequest request)
        {
            return ReturnActionResult(repository.Add(request));
        }

        [HttpDelete("remove_from_favorite")]
        public IActionResult Delete([FromForm] DeleteFavoriteRequest request)
        {
            return ReturnActionResult(repository.Delete(request));
        }


        [HttpGet("get_by_user_id")]
        public IActionResult GetByUserId([FromQuery(Name = "user_id")] int UserId)
        {
            return ReturnActionResult(repository.GetByUserId(UserId));
        }

        [HttpGet("get_by_favorite_id")]
        public IActionResult GetByFavoriteId([FromQuery(Name = "favorite_id")] int FavoriteId)
        {
            return ReturnActionResult(repository.GetByFavoriteId(FavoriteId));
        }


        [HttpDelete("clear_favorites")]
        public IActionResult Clear()
        {
            return ReturnActionResult(repository.Clear());
        }
    }
}
