using Quotes.DTO.Requests;
using Quotes.Helper;
using Quotes.Models;

namespace Quotes.Repositories.Interfaces
{
    public interface IUserRepository
    {
        OperationType Login(LoginRequest request);
        OperationType Register(RegisterRequest request);
        OperationType Update(UpdateUserRequest request);
        OperationType Delete(int Id);
        OperationType Retrieve(int Id);
        OperationType GetAll();
        OperationType GetById(int Id);
        OperationType Clear();

        string GenerateToken(string Email, int Id);
        string GenerateToken(User user);
        int? IsValideteToken(string token);

    }
}
