using HelloJobBackEnd.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloJobBackEnd.Entities
{
    public class Setting : BaseEntity
    {
        public string Key { get; set; }
        public string? Value { get; set; }

        [NotMapped]

        public IFormFile? Image { get; set; }
    }
}
