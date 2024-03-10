using HelloJobBackEnd.Entities.Base;
using HelloJobBackEnd.Utilities.Enum;

namespace HelloJobBackEnd.Entities
{
    public class WishListItem : BaseEntity
    {
        public int? CvId { get; set; }
        public Cv Cv { get; set; }
        public int WishListId { get; set; }
        public WishList WishList { get; set; }
        public int? VacansId { get; set; }
        public Vacans Vacans { get; set; }
        public bool IsLiked { get; set; }

    }
}
