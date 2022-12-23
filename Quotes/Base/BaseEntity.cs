
using Quotes.Helper;

namespace Quotes.Base
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CreatedBy = "";
            CreatedAt = DateTime.Now.ToString(Constants.TYPE_DATE_TIME_FORMATER);
            UpdatedBy = "";
            UpdatedAt = "";
            IsDelete = false;
        }

        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedAt { get; set; }
        public Boolean IsDelete { get; set; }

    }
}
