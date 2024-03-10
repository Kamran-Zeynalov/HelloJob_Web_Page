using HelloJobBackEnd.Entities.Base;

namespace HelloJobBackEnd.Entities
{
    public class Experience : BaseEntity
    {
        public string Name { get; set; }
        public List<Cv>? Cvs { get; set; }
    }
}
