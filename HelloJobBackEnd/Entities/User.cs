using HelloJobBackEnd.Utilities.Enum;
using Microsoft.AspNetCore.Identity;

namespace HelloJobBackEnd.Entities
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
        public List<Cv>? Cvs { get; set; }
        public List<Company>? Companies { get; set; }

    }
}

