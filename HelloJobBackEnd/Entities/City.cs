using HelloJobBackEnd.Entities.Base;

namespace HelloJobBackEnd.Entities
{
    public class City : BaseEntity
    {
        public string Name { get; set; }
        public List<Cv>? Cvs { get; set; }
    }
}
