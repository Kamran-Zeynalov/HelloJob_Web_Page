using HelloJobBackEnd.Entities.Base;

namespace HelloJobBackEnd.Entities
{
    public class WishList : BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public List<WishListItem> WishListItems { get; set; }
    }
}
