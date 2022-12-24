using Quotes.DTO.Requests;
using Quotes.Helper;
using Quotes.Models;

namespace Quotes.Repositories.Interfaces
{
    public interface IUserRepository
    {
        OperationType Login(LoginRequest request);
        OperationType Register(RegisterRequest request);
        OperationType Update(int Id, RegisterRequest request);
        OperationType Delete(int Id);
        OperationType GetAll();
        OperationType GetById(int Id);
        OperationType Clear();

        public string GenerateToken(string Email, int Id);
        public string GenerateToken(User user);
        public int? IsValideteToken(string token);

    }
}
