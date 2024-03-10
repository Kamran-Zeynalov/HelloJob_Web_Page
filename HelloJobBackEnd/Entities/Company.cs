using HelloJobBackEnd.Entities.Base;
using HelloJobBackEnd.Utilities.Enum;

namespace HelloJobBackEnd.Entities
{
    public class Company : BaseEntity
    {
        public string Image { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public List<Vacans> Vacans { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public OrderStatus Status { get; set; }

    }
}
