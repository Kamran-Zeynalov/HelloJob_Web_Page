using HelloJobBackEnd.Utilities.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloJobBackEnd.ViewModel
{
    public class CvVM
    {
        public int Id { get; set; }
        public IFormFile? Image { get; set; }
        public string? Images { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        [DataType(DataType.Date)]

        public DateTime BornDate { get; set; }
        public int CityId { get; set; }
        public int OperatingModeId { get; set; }
        public int ExperienceId { get; set; }
        public int Salary { get; set; } = 0;
        public string Position { get; set; }
        public int BusinessAreaId { get; set; }
        public int EducationId { get; set; }
        public bool DrivingLicense { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]

        public int Number { get; set; }
        public IFormFile? CvPDF { get; set; }
        public string? CvPDFs { get; set; }
        public OrderStatus Status { get; set; }
        public int Count { get; set; }
    }
}
