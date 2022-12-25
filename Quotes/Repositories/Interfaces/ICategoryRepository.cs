using Quotes.DTO.Requests.Other;
using Quotes.Helper;

namespace Quotes.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        OperationType Add(AddCategoryRequest request);
        OperationType Update(int Id, AddCategoryRequest request);
        OperationType Delete(int Id);
        OperationType GetAll();
        OperationType GetById(int Id);
        OperationType Clear();

     
    }
}
