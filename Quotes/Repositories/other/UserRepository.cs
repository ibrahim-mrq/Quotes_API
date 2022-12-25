﻿using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Quotes.DTO.Requests;
using Quotes.DTO.Responses;
using Quotes.Helper;
using Quotes.Models;
using Quotes.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
            if (!Constants.ValidateHash(request.Password + "", localUser.PasswordHash, localUser.PasswordSalt))
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

                Constants.GenerateHash($"{request.Password}", out hash, out salt);
                localUser.PasswordHash = hash;
                localUser.PasswordSalt = salt;
                localUser.UpdatedAt = DateTime.Now.ToString(Constants.TYPE_DATE_TIME_FORMATER);

                _dbContext.Users.Update(localUser);
                _dbContext.SaveChanges();
                return Constants.SuccessResponse("Register successfully", new { user = _map.Map<UserResponse>(localUser) });
            }
            else if (localUser != null && localUser.IsDelete == false)
            {
                return Constants.BadRequestResponse("Email Already Exist!", null);
            }
            var currentItem = _map.Map<User>(request);
            Constants.GenerateHash($"{request.Password}", out hash, out salt);
            currentItem.PasswordHash = hash;
            currentItem.PasswordSalt = salt;
            currentItem.Token = GenerateToken(currentItem);
            _dbContext.Users.Add(currentItem);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Register successfully", new { user = _map.Map<UserResponse>(currentItem) });
        }

        public OperationType Update(UpdateUserRequest request)
        {
            if (!Constants.InputValidation(request).Status)
            {
                return Constants.InputValidation(request);
            }
            var localItem = _dbContext.Users.Where(x => x.Id.Equals(request.Id) && x.IsDelete == false).SingleOrDefault();
            if (localItem == null)
            {
                return Constants.NotFoundResponse("User Id not Found!", null);
            }
            localItem.UpdatedAt = DateTime.Now.ToString(Constants.TYPE_DATE_TIME_FORMATER);
            var currentItem = _map.Map(request, localItem);
            _dbContext.Users.Update(currentItem);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Update User successfully", new { User = _map.Map<UserResponse>(currentItem) });
        }

        public OperationType Delete(int Id)
        {
            var localItem = _dbContext.Users.Where(x => x.Id.Equals(Id) && x.IsDelete == false).SingleOrDefault();
            if (localItem == null)
            {
                return Constants.NotFoundResponse("User Id Not Found!", null);
            }
            localItem.IsDelete = true;
            localItem.UpdatedAt = DateTime.Now.ToString(Constants.TYPE_DATE_TIME_FORMATER);
            _dbContext.Users.Update(localItem);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("User Deleted successfully", null);
        }

        public OperationType Retrieve(int Id)
        {
            var localItem = _dbContext.Users.Where(x => x.Id.Equals(Id)).SingleOrDefault();
            if (localItem == null)
            {
                return Constants.NotFoundResponse("User Id Not Found!", null);
            }
            else if (localItem.IsDelete == false)
            {
                return Constants.BadRequestResponse("The user already exists!", null);
            }
            localItem.IsDelete = false;
            localItem.UpdatedAt = DateTime.Now.ToString(Constants.TYPE_DATE_TIME_FORMATER);
            _dbContext.Users.Update(localItem);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("User Retrieved successfully", new { user = _map.Map<UserResponse>(localItem) });
        }

        public OperationType GetAll()
        {
            var localList = _dbContext.Users.Where(x => x.IsDelete == false).ToList();
            var filter = _map.Map<List<User>, List<UserResponse>>(localList);
            return Constants.SuccessResponse("successfull", new { Users = filter });
        }

        public OperationType GetById(int Id)
        {
            var localList = _dbContext.Users.Where(x => x.Id.Equals(Id) && x.IsDelete == false).FirstOrDefault();
            if (localList == null)
            {
                return Constants.NotFoundResponse("User Id not exists!", null);
            }
            return Constants.SuccessResponse("successfull", new { User = _map.Map<UserResponse>(localList) });
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
                        new Claim(JwtRegisteredClaimNames.Email , $"{user.Email}"),
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
