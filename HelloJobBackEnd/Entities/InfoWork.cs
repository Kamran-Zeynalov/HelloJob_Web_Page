using HelloJobBackEnd.Entities.Base;

namespace HelloJobBackEnd.Entities
{
    public class InfoWork : BaseEntity
    {
        public string? Info { get; set; }
        public int VacansId { get; set; }
        public Vacans Vacans { get; set; }
    }
}
