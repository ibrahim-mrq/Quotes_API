using Quotes.DTO.Requests.User;
using Quotes.Helper;
using Quotes.Models;

namespace Quotes.Repositories.Interfaces
{
    public interface IUserRepository
    {
        OperationType Login(LoginRequest request);
        OperationType Register(RegisterRequest request);
        OperationType ForgotPassword(ForgotPasswordRequest request);
        OperationType ResetPassword(ResetPasswordRequest request);
        OperationType Update(UpdateUserRequest request);
        OperationType Delete(int Id);
        OperationType Retrieve(int Id);
        OperationType GetAll();
        OperationType GetUser(int Id);
        User? GetUserById(int Id);

        OperationType Clear();

        string GenerateToken(User user);
        int? IsValideteToken(string token);

        void SendEmail(string To, string Body);
        void GenerateHash(string password, out byte[]? passwordHash, out byte[]? passwordSalt);
        bool ValidateHash(string password, byte[]? passwordHash, byte[]? passwordSalt);


    }
}
