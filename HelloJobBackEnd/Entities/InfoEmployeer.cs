using HelloJobBackEnd.Entities.Base;

namespace HelloJobBackEnd.Entities
{
    public class InfoEmployeer : BaseEntity
    {
        public string? Info { get; set; }
        public int VacansId { get; set; }
        public Vacans Vacans { get; set; }
    }
}
