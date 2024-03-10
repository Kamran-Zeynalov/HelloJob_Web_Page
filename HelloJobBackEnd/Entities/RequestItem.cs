using HelloJobBackEnd.Entities.Base;
using HelloJobBackEnd.Utilities.Enum;

namespace HelloJobBackEnd.Entities
{
    public class RequestItem : BaseEntity
    {
        public int? CvId { get; set; }
        public int RequestId { get; set; }
        public Cv Cv { get; set; }
        public Request Request { get; set; }
        public Vacans Vacans { get; set; }
        public int VacansId { get; set; }
        public OrderStatus Status { get; set; }

    }
}
