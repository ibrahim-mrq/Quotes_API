using Microsoft.AspNetCore.Mvc;
using Quotes.Base;
using Quotes.Repositories.Interfaces;
using Quotes.DTO.Requests.Other;

namespace Quotes.Controllers
{
    //  [Route("api/[controller]")]
    [Route("api/quote")]
    [ApiController]
    //  [Consumes("application/json")]
    public class QuoteController : BaseController
    {

        private readonly IQuoteRepository _quoteRepository;

        public QuoteController(IQuoteRepository _quoteRepository)
        {
            this._quoteRepository = _quoteRepository;
        }


        [HttpPost("add_quote")]
        public IActionResult AddQuote([FromForm] AddQuoteRequest addRequest)
        {
            return ReturnActionResult(_quoteRepository.Add(addRequest));
        }

        [HttpPut("update_quote/{QuoteId}")]
        public IActionResult UpdateQuote([FromRoute] int QuoteId, [FromForm] AddQuoteRequest Request)
        {
            return ReturnActionResult(_quoteRepository.Update(QuoteId, Request));
        }

        [HttpDelete("remove_quote/{QuoteId}")]
        public IActionResult RemoveQuote([FromRoute] int QuoteId)
        {
            return ReturnActionResult(_quoteRepository.Delete(QuoteId));
        }


        [HttpGet("get_quotes")]
        public IActionResult GetQuotes()
        {
            return ReturnActionResult(_quoteRepository.GetAll());
        }

        [HttpGet("get_quote_by_id")]
        public IActionResult GetQuoteById([FromQuery(Name = "quote_id")] int QuoteId)
        {
            return ReturnActionResult(_quoteRepository.GetById(QuoteId));
        }

        [HttpGet("get_quote_by_author_id")]
        public IActionResult GetQuoteByAuthorId([FromQuery(Name = "author_id")] int AuthorId)
        {
            return ReturnActionResult(_quoteRepository.GetQuoteByAuthorId(AuthorId));
        }

        [HttpGet("get_quote_by_category_id")]
        public IActionResult GetQuoteByCategoryId([FromQuery(Name = "category_id")] int CategoryId)
        {
            return ReturnActionResult(_quoteRepository.GetQuoteByCategoryId(CategoryId));
        }


        [HttpDelete("clear_quotes")]
        public IActionResult ClearQuotes()
        {
            return ReturnActionResult(_quoteRepository.Clear());
        }


    }
}
