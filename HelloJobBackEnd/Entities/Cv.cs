using HelloJobBackEnd.Entities.Base;
using HelloJobBackEnd.Utilities.Enum;

namespace HelloJobBackEnd.Entities
{
    public class Cv : BaseEntity
    {
        public string Image { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string Position { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public DateTime BornDate { get; set; }
        public int OperatingModeId { get; set; }
        public OperatingMode OperatingMode { get; set; }
        public int ExperienceId { get; set; }
        public Experience Experience { get; set; }
        public int Salary { get; set; }
        public int BusinessAreaId { get; set; }
        public BusinessArea BusinessArea { get; set; }
        public int EducationId { get; set; }

        public Education Education { get; set; }
        public bool DrivingLicense { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime EndedAt { get; set; }
        public string Email { get; set; }
        public int Number { get; set; }
        public string CvPDF { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public OrderStatus Status { get; set; }
        public bool TimeIsOver { get; set; } = false;
        public int Count { get; set; }
        public List<Request>? Requests { get; set; }
        public List<WishListItem> WishListItems { get; set; }

    }
}
