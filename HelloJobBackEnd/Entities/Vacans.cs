using HelloJobBackEnd.Entities.Base;
using HelloJobBackEnd.Utilities.Enum;

namespace HelloJobBackEnd.Entities
{
    public class Vacans : BaseEntity
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public string Position { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public int OperatingModeId { get; set; }
        public OperatingMode OperatingMode { get; set; }
        public int ExperienceId { get; set; }
        public Experience Experience { get; set; }
        public int? Salary { get; set; }
        public int BusinessAreaId { get; set; }
        public BusinessArea BusinessArea { get; set; }
        public int EducationId { get; set; }

        public Education Education { get; set; }
        public bool DrivingLicense { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime EndedAt { get; set; }
        public List<InfoWork>? InfoWorks { get; set; }
        public List<InfoEmployeer>? infoEmployeers { get; set; }
        public bool TimeIsOver { get; set; } = false;

        public OrderStatus Status { get; set; }
        public int Count { get; set; }
        public List<Request>? Requests { get; set; }
        public List<WishListItem> WishListItems { get; set; }


    }
}
