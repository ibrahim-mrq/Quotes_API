using System.Text;

namespace Quotes.Helper
{
    public class Constants
    {

        public static readonly string TYPE_LOGO = "https://www.wepal.net/ar/uploads/2732018-073911PM-1.jpg";
        public static readonly string TYPE_LOCAL_URL = "https://localhost:7194/Images/";


        public static OperationType InputValidation(int value)
        {
            var operationType = new OperationType()
            {
                Status = true,
                Message = "Invalid Input Data!",
                Code = 422
            };

            if (value <= 0 || value == null)
            {
                operationType. Message = "The " + value + " can't be null or blank!";
                operationType.Status = false;
            }

            return operationType;
        }
        public static OperationType InputValidation(object model)
        {
            var operationType = new OperationType()
            {
                Status = true,
                Message = "Invalid Input Data!",
                Code = 422
            };

            var errorist = new List<string>();
            foreach (var item in model.GetType().GetProperties())
            {
                if (item.PropertyType == typeof(string))
                {
                    var propStringValue = item.GetValue(model, null) as string;
                    if (string.IsNullOrEmpty(propStringValue))
                    {
                        errorist.Add("The " + item.Name + " can't be null or blank!");
                        operationType.Status = false;
                    }
                }
                if (item.PropertyType == typeof(int?))
                {
                    var propIntValue = item.GetValue(model, null) as int?;
                    if (propIntValue <= 0 || propIntValue == null)
                    {
                        errorist.Add("The " + item.Name + " can't be null or blank!");
                        operationType.Status = false;
                    }
                }
            }
            operationType.Data = new { error = errorist };
            return operationType;
        }

        public static OperationType InputLength(object model, int Length)
        {
            var operationType = new OperationType()
            {
                Status = true,
                Message = "Invalid Input Data!",
                Code = 422
            };
            var errorist = new List<string>();
            foreach (var item in model.GetType().GetProperties())
            {
                if (item.PropertyType == typeof(int?))
                {
                    var propIntValue = item.GetValue(model, null) as int?;
                    if (propIntValue != null)
                    {
                        if (propIntValue.GetValueOrDefault().ToString().Length != Length)
                        {
                            errorist.Add(item.Name + $" must be {Length} characters long!");
                            operationType.Status = false;
                        }
                    }
                }
            }
            operationType.Data = new { error = errorist };
            return operationType;
        }


        public static OperationType SuccessResponse(String message, Object? data)
        {
            return new OperationType()
            {
                Status = true,
                Message = message,
                Code = 200,
                Data = data
            };
        }
        public static OperationType UnprocessableEntityResponse(String message, Object? data)
        {
            return new OperationType()
            {
                Status = false,
                Message = message,
                Code = 422,
                Data = data
            };
        }
        public static OperationType BadRequestResponse(String message, Object? data)
        {
            return new OperationType()
            {
                Status = false,
                Message = message,
                Code = 400,
                Data = data
            };
        }
        public static OperationType NotFoundResponse(String message, Object? data)
        {
            return new OperationType()
            {
                Status = false,
                Message = message,
                Code = 404,
                Data = data
            };
        }


        public static void GenerateHash(String password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hash = new System.Security.Cryptography.HMACSHA512())
            {
                passwordHash = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                passwordSalt = hash.Key;
            }
        }

        public static Boolean ValidateHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
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
