using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Services.Interface;
using HelloJobBackEnd.Utilities.Enum;
using HelloJobBackEnd.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace HelloJobBackEnd.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly HelloJobDbContext _context;

        public CompanyService(HelloJobDbContext context)
        {
            _context = context;
        }

        public List<Company> GetTopAcceptedCompaniesWithVacans(int? count = null)
        {
            IQueryable<Company> query = _context.Companies
                .Include(v => v.Vacans).ThenInclude(x => x.BusinessArea)
                    .Include(v => v.Vacans).ThenInclude(x => x.BusinessArea.BusinessTitle)
                    .Include(v => v.Vacans.Where(v => v.TimeIsOver == false))
                .OrderByDescending(c => c.Vacans.Count);

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            List<Company> companies = query.ToList();
            return companies;
        }

        public Company? GetCompanyWithVacansById(int id)
        {

            Company? company = _context.Companies.Include(v => v.Vacans).
                Include(x => x.Vacans).ThenInclude(x => x.WishListItems).ThenInclude(x => x.WishList).ThenInclude(x => x.User).
                Include(x => x.User)
                 .Include(v => v.Vacans.Where(v => v.TimeIsOver == false)).
            ThenInclude(b => b.BusinessArea).ThenInclude(x => x.BusinessTitle).
                FirstOrDefault(x => x.Id == id);
            return company;
        }
        public CompanyVM GetCompanyById(int id)
        {
            CompanyVM? companyVM = _context.Companies
                .Include(v => v.Vacans)
                .Where(p => p.Id == id)
                .Select(p => new CompanyVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Email = p.Email,
                    Images = p.Image
                })
                .FirstOrDefault(x => x.Id == id);

            return companyVM;
        }
    }
}
