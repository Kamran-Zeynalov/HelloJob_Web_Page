using System.ComponentModel.DataAnnotations;

namespace HelloJobBackEnd.ViewModel
{
    public class LoginVM
    {
        [Display(Name = "İstifadəçi adı")]
        public string UserName { get; set; }

        [Display(Name = "Şifrə")]

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
