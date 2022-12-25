using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quotes.Base;
using Quotes.DTO.Requests.Other;
using Quotes.Repositories.Interfaces;

namespace Quotes.Controllers
{
    //  [Route("api/[controller]")]
    [Route("api/category")]
    [ApiController]
    public class CategoryController : BaseController
    {
        private readonly ICategoryRepository repository;

        public CategoryController(ICategoryRepository repository)
        {
            this.repository = repository;
        }


        [HttpPost("add_category")]
        public IActionResult AddCategory([FromForm] AddCategoryRequest request)
        {
            return ReturnActionResult(repository.Add(request));
        }

        [HttpPut("update_category/{CategoryId}")]
        public IActionResult UpdateCategory([FromRoute] int CategoryId, [FromForm] AddCategoryRequest request)
        {
            return ReturnActionResult(repository.Update(CategoryId, request));
        }

        [HttpDelete("remove_category/{CategoryId}")]
        public IActionResult DeleteQuote([FromRoute] int CategoryId)
        {
            return ReturnActionResult(repository.Delete(CategoryId));
        }


        [HttpGet("get_all_categories")]
        public IActionResult GetCategories()
        {
            return ReturnActionResult(repository.GetAll());
        }

        [HttpGet("get_category_by_id")]
        public IActionResult GetQuoteById([FromQuery(Name = "category_id")] int CategoryId)
        {
            return ReturnActionResult(repository.GetById(CategoryId));
        }


        [HttpDelete("clear_categories")]
        public IActionResult ClearCategories()
        {
            return ReturnActionResult(repository.Clear());
        }


    }
}
