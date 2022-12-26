﻿using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Quotes.DTO.Responses;
using Quotes.Helper;
using Quotes.Models;
using Quotes.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using Quotes.DTO.Requests.User;
using Twilio.Rest.Verify.V2.Service;
using Twilio;

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
            /*if (!Constants.IsValidEmail(request.Email))
            {
                return Constants.UnprocessableEntityResponse("Please use a valid email!", null);
            }*/

            var localUser = _dbContext.Users.Where(x => x.Email == request.Email && x.IsDelete == false).FirstOrDefault();
            if (localUser == null)
            {
                return Constants.NotFoundResponse("User Not Found!", null);
            }
            if (!ValidateHash(request.Password + "", localUser.PasswordHash, localUser.PasswordSalt))
            {
                return Constants.UnprocessableEntityResponse("The password is incorrect!", null); ;
            }
            localUser.DeviceToken = request.DeviceToken;
            localUser.DeviceType = request.DeviceType;
            localUser.Token = GenerateToken(localUser);
            localUser.ExpirationToken = DateTime.Now.AddMonths(12);

            _dbContext.Users.Update(localUser);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Login successfully", new { user = _map.Map<UserResponse>(localUser) });
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
                localUser.ExpirationToken = DateTime.Now.AddMonths(12);

                GenerateHash($"{request.Password}", out hash!, out salt!);
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
            GenerateHash($"{request.Password}", out hash!, out salt!);
            currentItem.PasswordHash = hash;
            currentItem.PasswordSalt = salt;
            currentItem.Token = GenerateToken(currentItem);
            currentItem.ExpirationToken = DateTime.Now.AddMonths(12);
            _dbContext.Users.Add(currentItem);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Register successfully", new { user = _map.Map<UserResponse>(currentItem) });
        }

        public OperationType ForgotPassword(ForgotPasswordRequest request)
        {
            if (!Constants.InputValidation(request).Status)
            {
                return Constants.InputValidation(request);
            }
            var localUser = _dbContext.Users.Where(x => x.Email == request.Email && x.IsDelete == false).FirstOrDefault();
            if (localUser == null)
            {
                return Constants.NotFoundResponse("Email Not Found!", null);
            }
            /* localUser.Code = Constants.ValideResetPasswordCode();
             localUser.ExpirationCode = DateTime.Now.AddMinutes(2);
             _dbContext.Users.Update(localUser);
             _dbContext.SaveChanges();
             SendEmail("abo.mahroq@gmail.com", $"Your Reste Code is: {localUser.Code}\n you have 2 minite to ues it.");*/
            SendPhone("asd");
            return Constants.SuccessResponse("check your email now", null);
        }

        public OperationType ResetPassword(ResetPasswordRequest request)
        {
            if (!Constants.InputValidation(request).Status)
            {
                return Constants.InputValidation(request);
            }
            var localUser = _dbContext.Users.Where(x => x.Email == request.Email && x.IsDelete == false).FirstOrDefault();
            if (localUser == null)
            {
                return Constants.NotFoundResponse("Email Not Found!", null);
            }
            if (request.Code != localUser.Code)
            {
                return Constants.NotFoundResponse("Invalid Code!", null);
            }
            if (localUser.ExpirationCode < DateTime.Now)
            {
                return Constants.NotFoundResponse("Expiration Code!", null);
            }
            GenerateHash($"{request.Password}", out byte[]? hash, out byte[]? salt);
            localUser.PasswordHash = hash;
            localUser.PasswordSalt = salt;
            localUser.Token = "";
            localUser.Code = "";
            localUser.ExpirationToken = DateTime.Now;
            localUser.ExpirationCode = DateTime.Now;
            _dbContext.Users.Update(localUser);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Reset Password successfully", null);
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

        public OperationType GetUser(int Id)
        {
            var localList = _dbContext.Users.Where(x => x.Id.Equals(Id) && x.IsDelete == false).FirstOrDefault();
            if (localList == null)
            {
                return Constants.NotFoundResponse("User Id not exists!", null);
            }
            return Constants.SuccessResponse("successfull", new { User = _map.Map<UserResponse>(localList) });
        }

        public User? GetUserById(int Id)
        {
            var localList = _dbContext.Users.Where(x => x.Id.Equals(Id) && x.IsDelete == false).FirstOrDefault();
            if (localList == null)
            {
                return null;
            }
            return localList;
        }

        public OperationType Clear()
        {
            var list = _dbContext.Users.ToList();
            _dbContext.Users.RemoveRange(list);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("All Users have been successfully deleted!", null);
        }


        public string GenerateToken(User user)
        {
            var key = Encoding.ASCII.GetBytes("LZImjD2eUbUxhxjIdyOJuYT4FjWhKSJy");
            var descriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.Now.AddMonths(12),
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


        public void SendEmail(string To, string Body)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("seven.arts.ku@gmail.com"));
            email.To.Add(MailboxAddress.Parse(To));
            email.Subject = "Quites App, Reset Password";
            email.Body = new TextPart(TextFormat.Plain) { Text = Body };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            // smtp.Connect("smtp.live.com", 587, SecureSocketOptions.StartTls);
            // smtp.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
            // smtp.Connect("email-smtp.[AWS REGION].amazonaws.com", 587, SecureSocketOptions.StartTls);

            smtp.Authenticate("seven.arts.ku@gmail.com", "fneyqxlrzjjdqbsi");
            smtp.Send(email);
            smtp.Disconnect(true);

        }
        public void SendPhone(string Phone)
        {
            try
            {
                TwilioClient.Init("abo.mahroq@gmail.com", "aTSXZ!5&8=9dGe22");
                var verificationCheck = VerificationCheckResource.Create(
                    to: "+972592440530",
                    code: "123456",
                    pathServiceSid: "VAXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"
                );
                Console.WriteLine(verificationCheck.Status);
            }
            catch (Exception e)
            {
                Console.WriteLine($"error: {e.Message}");
            }
        }

        public void GenerateHash(String password, out byte[]? passwordHash, out byte[]? passwordSalt)
        {
            using var hash = new System.Security.Cryptography.HMACSHA512();
            passwordHash = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            passwordSalt = hash.Key;
        }
        public Boolean ValidateHash(string password, byte[]? passwordHash, byte[]? passwordSalt)
        {
            if (passwordHash == null || passwordSalt == null)
            {
                return false;
            }
            using var hash = new System.Security.Cryptography.HMACSHA512(passwordSalt);
            var newPassHash = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < newPassHash.Length; i++)
            {
                if (newPassHash[i] != passwordHash[i]) return false;
            }
            return true;
        }


    }
}
