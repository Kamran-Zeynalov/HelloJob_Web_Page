using HelloJobBackEnd.Entities.Base;

namespace HelloJobBackEnd.Entities
{
    public class OperatingMode : BaseEntity
    {
        public string Name { get; set; }
        public List<Cv>? Cvs { get; set; }
        public List<Vacans>? Vacans { get; set; }

    }
}
