using HelloJobBackEnd.Utilities.Enum;
using System.ComponentModel.DataAnnotations;

namespace HelloJobBackEnd.ViewModel
{
    public class CompanyVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = " Şirkət logosu qeyd olunmalıdır.*")]
        public IFormFile Image { get; set; }
        public string? Images { get; set; }
        [Required(ErrorMessage = " Şirkət adı qeyd olunmalıdır.*")]
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "E-poçt qeyd olunmalıdır.*")]
        public string Email { get; set; }
        public OrderStatus Status { get; set; }
    }
}
