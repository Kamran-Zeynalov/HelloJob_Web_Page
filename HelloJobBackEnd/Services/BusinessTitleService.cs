using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Services.Interface;
using HelloJobBackEnd.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace HelloJobBackEnd.Services
{
    public class BusinessTitleService : IBusinessTitleService
    {
        private readonly HelloJobDbContext _context;

        public BusinessTitleService(HelloJobDbContext context)
        {
            _context = context;
        }

        public List<BusinessTitle> GetAllBusinessTitlesWithAreas()
        {
            List<BusinessTitle> titles = _context.BusinessTitle.Include(b => b.BusinessAreas).
                Include(x => x.BusinessAreas).ThenInclude(x => x.Vacans).
                        Include(x => x.BusinessAreas).ThenInclude(x => x.Cvs)
                .ToList();

            return titles;
        }

        public BusinessTitleVM EditedTItle(int id)
        {
            BusinessTitleVM? titleVM = _context.BusinessTitle.Include(v => v.BusinessAreas).Select(p =>
                                              new BusinessTitleVM
                                              {
                                                  Id = p.Id,
                                                  Name = p.Name,
                                                  Images = p.Image

                                              }).FirstOrDefault(x => x.Id == id);
            return titleVM;
        }

        public BusinessTitle GetTitleById(int id)
        {
            BusinessTitle? title = _context.BusinessTitle
             .Include(x => x.BusinessAreas)
             .FirstOrDefault(s => s.Id == id);
            return title;
        }
    }
}
