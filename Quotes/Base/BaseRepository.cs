using Quotes.Helper;

namespace Quotes.Base
{
    public class BaseRepository
    {

        public OperationType InputValidation(object model)
        {
            var operationType = new OperationType()
            {
                Status = true,
                Message = "",
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



    }
}
