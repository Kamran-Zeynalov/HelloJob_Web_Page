using HelloJobBackEnd.Entities.Base;

namespace HelloJobBackEnd.Entities
{
    public class BusinessArea : BaseEntity
    {
        public string Name { get; set; }
        public List<Cv>? Cvs { get; set; }
        public List<Vacans>? Vacans { get; set; }

        public int BusinessTitleId { get; set; }
        public BusinessTitle BusinessTitle { get; set; }


    }
}
