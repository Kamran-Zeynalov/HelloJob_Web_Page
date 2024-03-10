using HelloJobBackEnd.Entities.Base;

namespace HelloJobBackEnd.Entities
{
    public class Education : BaseEntity
    {
        public string Name { get; set; }
        public List<Cv>? Cvs { get; set; }
    }
}
