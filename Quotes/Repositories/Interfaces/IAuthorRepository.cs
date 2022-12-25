using Quotes.DTO.Requests.Other;
using Quotes.Helper;

namespace Quotes.Repositories.Interfaces
{
    public interface IAuthorRepository
    {
        OperationType Add(AddAuthorRequest request);
        OperationType Update(int Id, AddAuthorRequest request);
        OperationType Delete(int Id);
        OperationType GetAll();
        OperationType GetById(int Id);
        OperationType Clear();
    }
}
