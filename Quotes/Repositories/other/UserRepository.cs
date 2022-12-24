using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Quotes.DTO.Requests;
using Quotes.DTO.Responses;
using Quotes.Helper;
using Quotes.Models;
using Quotes.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;

namespace Quotes.Repositories.other
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _dbContext;
        private readonly IMapper _map;

        public UserRepository(DBContext dBContext, IMapper map)
        {
            this._dbContext = dBContext;
            this._map = map;
        }

        public OperationType Login(LoginRequest request)
        {
            if (!Constants.InputValidation(request).Status)
            {
                return Constants.InputValidation(request);
            }
            var localUser = _dbContext.Users.Where(x => x.Email == request.Email && x.IsDelete == false).FirstOrDefault();
            if (localUser == null)
            {
                return Constants.NotFoundResponse("User Not Found!", null);
            }
            if (!Constants.ValidateHash(request.Password, localUser.PasswordHash, localUser.PasswordSalt))
            {
                return Constants.UnprocessableEntityResponse("The password is incorrect!", null); ;
            }
            localUser.DeviceToken = request.DeviceToken;
            localUser.DeviceType = request.DeviceType;
            localUser.Token = GenerateToken(localUser);

            _dbContext.Users.Update(localUser);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Add Author successfully", new { user = _map.Map<UserResponse>(localUser) });
        }

        public OperationType Register(RegisterRequest request)
        {
            if (!Constants.InputValidation(request).Status)
            {
                return Constants.InputValidation(request);
            }
            if (!Constants.InputValidationPasswordLength($"{request.Password}").Status)
            {
                return Constants.InputValidationPasswordLength($"{request.Password}");
            }
            byte[] hash, salt;
            var localUser = _dbContext.Users.Where(x => x.Email == request.Email).FirstOrDefault();
            if (localUser != null && localUser.IsDelete == true)
            {
                localUser.IsDelete = false;
                localUser.Name = request.Name;
                localUser.DeviceToken = request.DeviceToken;
                localUser.DeviceType = request.DeviceType;
                localUser.Token = GenerateToken(localUser);

                Constants.GenerateHash(request.Password, out hash, out salt);
                localUser.PasswordHash = hash;
                localUser.PasswordSalt = salt;

                _dbContext.Users.Update(localUser);
                _dbContext.SaveChanges();
                return Constants.SuccessResponse("Register successfully", new { user = _map.Map<UserResponse>(localUser) });
            }
            else if (localUser != null && localUser.IsDelete == false)
            {
                return Constants.BadRequestResponse("Email Already Exist!", null);
            }
            var currentItem = _map.Map<User>(request);
            Constants.GenerateHash(request.Password, out hash, out salt);
            currentItem.PasswordHash = hash;
            currentItem.PasswordSalt = salt;
            currentItem.Token = GenerateToken(currentItem);
            _dbContext.Users.Add(currentItem);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Register successfully", new { user = _map.Map<UserResponse>(currentItem) });
        }

        public OperationType Update(int AuthorId, RegisterRequest request)
        {
            var localItem = _dbContext.Authors.Where(x => x.Id.Equals(AuthorId) && x.IsDelete == false).SingleOrDefault();
            if (localItem == null)
            {
                return Constants.NotFoundResponse("Author Id not exists!", null);
            }
            if (!Constants.InputValidation(request).Status)
            {
                return Constants.InputValidation(request);
            }
            if (!Constants.InputLength(request, 4).Status)
            {
                return Constants.InputLength(request, 4);
            }
            return Constants.SuccessResponse("Update Author successfully", null);
        }

        public OperationType Delete(int AuthorId)
        {
            var localItem = _dbContext.Authors.Where(x => x.Id.Equals(AuthorId) && x.IsDelete == false).SingleOrDefault();
            if (localItem == null)
            {
                return Constants.NotFoundResponse("Author Id Not Found!", null);
            }
            localItem.IsDelete = true;
            _dbContext.Authors.Update(localItem);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Author Deleted successfully", new { Author = _map.Map<AuthorResponse>(localItem) });
        }

        public OperationType GetAll()
        {
            var localList = _dbContext.Authors.Where(x => x.IsDelete == false).ToList();
            var filter = _map.Map<List<Author>, List<AuthorResponse>>(localList);
            return Constants.SuccessResponse("successfull", new { Authors = filter });
        }

        public OperationType GetById(int UserId)
        {
            var localList = _dbContext.Users.Where(x => x.Id.Equals(UserId) && x.IsDelete == false).FirstOrDefault();
            if (localList == null)
            {
                return Constants.NotFoundResponse("User Id not exists!", null);
            }
            return Constants.SuccessResponse("successfull", new { User = localList });
            //   return Constants.SuccessResponse("successfull", new { Author = _map.Map<AuthorResponse>(localList) });
        }

        public OperationType Clear()
        {
            var list = _dbContext.Users.ToList();
            _dbContext.Users.RemoveRange(list);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("All Users have been successfully deleted!", null);
        }


        public string GenerateToken(string Email, int Id)
        {
            var key = Encoding.ASCII.GetBytes("LZImjD2eUbUxhxjIdyOJuYT4FjWhKSJy");
            var descriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMonths(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
                Subject = new ClaimsIdentity(
                    new Claim[] {
                        new Claim(JwtRegisteredClaimNames.Email , Email),
                        new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                        new Claim("userId" , Id.ToString()),
                    }
                )
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

        public string GenerateToken(User user)
        {
            var key = Encoding.ASCII.GetBytes("LZImjD2eUbUxhxjIdyOJuYT4FjWhKSJy");
            var descriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMonths(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
                Subject = new ClaimsIdentity(
                    new Claim[] {
                        new Claim(JwtRegisteredClaimNames.Email , user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                        new Claim("userId" , user.Id.ToString()),
                    }
                )
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

        public int? IsValideteToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var key = Encoding.ASCII.GetBytes("LZImjD2eUbUxhxjIdyOJuYT4FjWhKSJy");
            var handler = new JwtSecurityTokenHandler();
            try
            {
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // ClockSkew = TimeSpan.FromMinutes(1),
                    ClockSkew = TimeSpan.Zero,

                }, out SecurityToken validetedToken);

                var jwtToken = (JwtSecurityToken)validetedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "userId").Value);
                return userId;
            }
            catch
            {
                return null;
            }
        }


    }
}
