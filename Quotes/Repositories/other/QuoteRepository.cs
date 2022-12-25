using AutoMapper;
using Quotes.Models;
using Quotes.DTO.Responses;
using Quotes.Helper;
using Quotes.Repositories.Interfaces;
using Quotes.DTO.Requests.Other;

namespace Resturants.Repositories.other
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly DBContext _dbContext;
        private readonly IMapper _map;

        public QuoteRepository(DBContext dBContext, IMapper map)
        {
            this._dbContext = dBContext;
            this._map = map;
        }


        public OperationType Add(AddQuoteRequest request)
        {
            if (!Constants.InputValidation(request).Status)
            {
                return Constants.InputValidation(request);
            }
            var localAuthor = _dbContext.Authors.Where(x => x.Id.Equals(request.AuthorId) && x.IsDelete == false).SingleOrDefault();
            if (localAuthor == null)
            {
                return Constants.NotFoundResponse("Author Id Not Found!", null);
            }
            var localCategory = _dbContext.Categories.Where(x => x.Id.Equals(request.CategoryId) && x.IsDelete == false).SingleOrDefault();
            if (localCategory == null)
            {
                return Constants.NotFoundResponse("Category Id Not Found!", null);
            }
            var currentItem = _map.Map<Quote>(request);
            currentItem.AuthorName = localAuthor.Name;
            currentItem.CategoryName = localCategory.Name;
            _dbContext.Quotes.Add(currentItem);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Add Quote successfully", new { Quote = _map.Map<QuoteResponse>(currentItem) });
        }

        public OperationType Update(int QuoteId, AddQuoteRequest request)
        {
            var localItem = _dbContext.Quotes.Where(x => x.Id.Equals(QuoteId) && x.IsDelete == false).SingleOrDefault();
            if (localItem == null)
            {
                return Constants.NotFoundResponse("Quote Id not exists!", null);
            }
            if (!Constants.InputValidation(request).Status)
            {
                return Constants.InputValidation(request);
            }
            var localAuthor = _dbContext.Authors.Where(x => x.Id.Equals(request.AuthorId) && x.IsDelete == false).SingleOrDefault();
            if (localAuthor == null)
            {
                return Constants.NotFoundResponse("Author Id Not Found!", null);
            }
            var localCategory = _dbContext.Categories.Where(x => x.Id.Equals(request.CategoryId) && x.IsDelete == false).SingleOrDefault();
            if (localCategory == null)
            {
                return Constants.NotFoundResponse("Category Id Not Found!", null);
            }
            localItem.UpdatedAt = DateTime.Now.ToString(Constants.TYPE_DATE_TIME_FORMATER);
            var currentItem = _map.Map(request, localItem);
            _dbContext.Quotes.Update(currentItem);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Update Quote successfully", new { Quote = _map.Map<QuoteResponse>(currentItem) });
        }

        public OperationType Delete(int QuoteId)
        {
            var localItem = _dbContext.Quotes.Where(x => x.Id.Equals(QuoteId) && x.IsDelete == false).SingleOrDefault();
            if (localItem == null)
            {
                return Constants.NotFoundResponse("Quote Id Not Found!", null);
            }
            localItem.IsDelete = true;
            _dbContext.Quotes.Update(localItem);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Quote Deleted successfully", new { Quote = _map.Map<QuoteResponse>(localItem) });
        }

        public OperationType GetAll()
        {
            var localList = _dbContext.Quotes.Where(x => x.IsDelete == false).ToList();
            var filter = _map.Map<List<Quote>, List<QuoteResponse>>(localList);
            return Constants.SuccessResponse("successfull", new { Quotes = filter });
        }

        public OperationType GetById(int QuoteId)
        {
            var localList = _dbContext.Quotes.Where(x => x.Id.Equals(QuoteId) && x.IsDelete == false).FirstOrDefault();
            if (localList == null)
            {
                return Constants.NotFoundResponse("Quote Id not exists!", null);
            }
            return Constants.SuccessResponse("successfull", new { Quote = _map.Map<QuoteResponse>(localList) });
        }

        public OperationType GetQuoteByAuthorId(int AuthorId)
        {
            var localList = _dbContext.Quotes.Where(x => x.AuthorId.Equals(AuthorId) && x.IsDelete == false).ToList();
            var filter = _map.Map<List<Quote>, List<QuoteResponse>>(localList);
            return Constants.SuccessResponse("successfull", new { Quote = filter });
        }

        public OperationType GetQuoteByCategoryId(int CategoryId)
        {
            var localList = _dbContext.Quotes.Where(x => x.CategoryId.Equals(CategoryId) && x.IsDelete == false).ToList();
            var filter = _map.Map<List<Quote>, List<QuoteResponse>>(localList);
            return Constants.SuccessResponse("successfull", new { Quote = filter });
        }

        public OperationType Clear()
        {
            var Quotes = _dbContext.Quotes.ToList();
            _dbContext.Quotes.RemoveRange(Quotes);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("All Quotes have been successfully deleted!", null);
        }


    }
}
