using Quotes.DTO.Requests.User;
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
        OperationType GetUser(int Id);
        User? GetUserById(int Id);

        OperationType Clear();

        string GenerateToken(User user);
        int? IsValideteToken(string token);

        void SendEmail(string From, string To, string Subject, string Body);

    }
}
