using HelloJobBackEnd.Entities.Base;

namespace HelloJobBackEnd.Entities
{
    public class Rules : BaseEntity
    {
        public bool CV { get; set; }
        public bool Vacans { get; set; }

        public string Rule { get; set; }
    }
}
