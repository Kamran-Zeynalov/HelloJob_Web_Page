using HelloJobBackEnd.Entities.Base;
using HelloJobBackEnd.Utilities.Enum;

namespace HelloJobBackEnd.Entities
{
    public class Request : BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public List<RequestItem> RequestItems { get; set; }
    }
}
