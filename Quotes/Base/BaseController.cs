using Microsoft.AspNetCore.Mvc;
using Quotes.Helper;

namespace Quotes.Base
{
    public class BaseController : ControllerBase
    {

        public IActionResult ReturnActionResult(OperationType respone)
        {
            return respone.Code switch
            {
                200 => Ok(respone),
                400 => BadRequest(respone),
                404 => NotFound(respone),
                422 => UnprocessableEntity(respone),
                _ => NoContent(),
            };
        }

        public int GetUserId()
        {
            var userId = HttpContext.Items["userId"];
            int Id = 0;
            if (userId != null)
            {
                Id = (int)userId;
            }
            return Id;
        }

    }
}
