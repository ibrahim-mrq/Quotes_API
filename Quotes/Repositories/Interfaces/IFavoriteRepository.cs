using Quotes.DTO.Requests.Other;
using Quotes.Helper;

namespace Quotes.Repositories.Interfaces
{
    public interface IFavoriteRepository
    {
        OperationType Add(AddFavoriteRequest request);
        OperationType Delete(DeleteFavoriteRequest request);
        OperationType GetByUserId(int Id);
        OperationType GetByFavoriteId(int Id);
        OperationType Clear();
    }
}
