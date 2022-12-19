using Quotes.DTO.Requests;
using Quotes.Helper;

namespace Quotes.Repositories.Interfaces
{
    public interface IQuoteRepository
    {

        OperationType Add(AddQuoteRequest request);
        OperationType Update(int Id, AddQuoteRequest request);
        OperationType Delete(int Id);
        OperationType GetAll();
        OperationType GetById(int Id);
        OperationType GetQuoteByAuthorId(int AuthorId);
        OperationType GetQuoteByCategoryId(int CategoryId);
        OperationType Clear();

    }
}
