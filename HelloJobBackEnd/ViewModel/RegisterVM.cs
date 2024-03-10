using HelloJobBackEnd.Utilities.Enum;
using System.ComponentModel.DataAnnotations;

namespace HelloJobBackEnd.ViewModel
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Adınız qeyd olunmalıdır.*")]
        [Display(Name = "Adınız")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Soyad  qeyd edilməlidir.*")]
        [Display(Name = "Soyadınız")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "İstifadəçi adı qeyd edilməlidir.*")]
        [Display(Name = "İstifadəçi adı")]
        public string Username { get; set; }

        [Required(ErrorMessage = "E-poçt  qeyd edilməlidir.*")]
        [Display(Name = "E-poçt ")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifrə  qeyd edilməlidir.*")]
        [Display(Name = "Şifrə")]
        public string Password { get; set; }
        [DataType(DataType.Password), Compare(nameof(Password))]

        [Required(ErrorMessage = "Şifrə  İlə eyni qeyd edilməlidir.*")]
        [Display(Name = "Şifrə təkrar")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Kateqoriya qeyd edilməlidir.*")]
        [Display(Name = "Kateqoriya")]
        public UserRole userRole { get; set; }

        public AdminRoles adminRoles { get; set; }


        [Required(ErrorMessage = "Qeydiyyatı Təsdiqlə.*")]
        public bool Terms { get; set; }
    }
}
