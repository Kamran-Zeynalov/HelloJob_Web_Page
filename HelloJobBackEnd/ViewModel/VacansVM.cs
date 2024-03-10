using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Utilities.Enum;
using System.ComponentModel.DataAnnotations;

namespace HelloJobBackEnd.ViewModel
{
    public class VacansVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = " Şəhər  qeyd olunmalıdır.*")]
        public int CityId { get; set; }
        [Required(ErrorMessage = " İş saatı  qeyd olunmalıdır.*")]

        public int OperatingModeId { get; set; }
        [Required(ErrorMessage = " İş stajı  qeyd olunmalıdır.*")]

        public int ExperienceId { get; set; }
        public int? Salary { get; set; } = 0;
        [Required(ErrorMessage = " Vəzifə  qeyd olunmalıdır.*")]

        public string Position { get; set; }
        [Required(ErrorMessage = " Kateqoriya  qeyd olunmalıdır.*")]

        public int BusinessAreaId { get; set; }
        [Required(ErrorMessage = " Təhsil  qeyd olunmalıdır.*")]

        public int EducationId { get; set; }
        [Required(ErrorMessage = " Şirkət  qeyd olunmalıdır.*")]

        public int CompanyId { get; set; }
        public bool DrivingLicense { get; set; }
        [Required(ErrorMessage = " İş barədə məlumat  qeyd olunmalıdır.*")]

        public string? InfoWorks { get; set; }
        public List<InfoWork>? AllWorkInfos { get; set; }
        public List<int>? DeleteWork { get; set; }

        [Required(ErrorMessage = " Namizəddən tələblər   qeyd olunmalıdır.*")]
        public string? infoEmployeers { get; set; }
        public List<int>? DeleteEmployeers { get; set; }
        public List<InfoEmployeer>? AllEmployeerInfos { get; set; }
        public OrderStatus Status { get; set; }


        public VacansVM()
        {
            AllWorkInfos = new();
            AllEmployeerInfos = new();
            DeleteWork = new List<int>();
            DeleteEmployeers = new List<int>();
        }
    }
}
