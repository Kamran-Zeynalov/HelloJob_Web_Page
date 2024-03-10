using HelloJobBackEnd.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloJobBackEnd.Entities
{
    public class BusinessTitle : BaseEntity
    {
        public string? Image { get; set; }

        public string Name { get; set; }
        public List<BusinessArea> BusinessAreas { get; set; }

    }
}
