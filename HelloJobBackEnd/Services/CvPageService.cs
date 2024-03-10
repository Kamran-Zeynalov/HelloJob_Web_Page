using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Services.Interface;
using HelloJobBackEnd.Utilities.Enum;
using HelloJobBackEnd.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

namespace HelloJobBackEnd.Services
{
    public class CvPageService : ICvPageService
    {
        private readonly HelloJobDbContext _context;

        public CvPageService(HelloJobDbContext context)
        {
            _context = context;
        }

        public IQueryable<Cv> GetAllCvs()
        {
            IQueryable<Cv> cvs = _context.Cvs
                 .Include(v => v.BusinessArea)
                 .Include(e => e.Education)
                 .Include(e => e.Experience)
                 .Include(c => c.City)
                 .Include(c => c.BusinessArea)
                 .Include(o => o.OperatingMode)
                 .Include(x => x.User)
                 .Include(x => x.WishListItems).ThenInclude(wt => wt.WishList)
                 .Include(x => x.WishListItems).ThenInclude(wt => wt.WishList.User)
                 .AsNoTracking();
            return cvs;
        }

        public async Task<List<Cv>> GetSortedCvs(string sort)
        {
            IQueryable<Cv> allCvs = GetAllCvs().Where(c => c.Status == OrderStatus.Accepted);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "new":
                        allCvs = allCvs.OrderByDescending(c => c.Id);
                        break;
                    case "old":
                        allCvs = allCvs.OrderBy(c => c.Id);
                        break;
                    case "salary_desc":
                        allCvs = allCvs.OrderByDescending(c => c.Salary);
                        break;
                    case "salary_asc":
                        allCvs = allCvs.OrderBy(c => c.Salary);
                        break;
                }
            }

            return await allCvs.ToListAsync();
        }

        public async Task<List<Cv>> GetFilteredData(int[] businessIds, int[] modeIds, int[] educationIds, int[] experienceIds, bool? hasDriverLicense)
        {
            IQueryable<Cv> allCvs = GetAllCvs().Where(c => c.Status == OrderStatus.Accepted);
            List<Cv> filteredCvs = ApplyFilters(allCvs, businessIds, modeIds, educationIds, experienceIds, hasDriverLicense);
            return filteredCvs.ToList();
        }




        private List<Cv> ApplyFilters(IQueryable<Cv> cvs, int[] businessIds, int[] modeIds, int[] educationIds, int[] experienceIds, bool? hasDriverLicense)
        {
            if (businessIds?.Length > 0)
            {
                cvs = cvs.Where(c => businessIds.Contains(c.BusinessArea.BusinessTitleId));
            }

            if (modeIds?.Length > 0)
            {
                cvs = cvs.Where(c => modeIds.Contains(c.OperatingMode.Id));

            }

            if (educationIds?.Length > 0)
            {
                cvs = cvs.Where(c => educationIds.Contains(c.Education.Id));

            }

            if (experienceIds?.Length > 0)
            {
                cvs = cvs.Where(c => experienceIds.Contains(c.Experience.Id));

            }

            if (hasDriverLicense.HasValue)
            {
                cvs = cvs.Where(c => c.DrivingLicense == hasDriverLicense.Value);

            }

            return cvs.ToList();
        }



        public Cv Details(int id)
        {
            Cv? cv = _context.Cvs.Include(v => v.BusinessArea)
              .Include(e => e.Education)
                   .Include(e => e.Experience)
                   .Include(c => c.City)
                   .Include(c => c.BusinessArea)
                   .Include(o => o.OperatingMode)
                   .Include(x => x.User).
                 Include(x => x.WishListItems).ThenInclude(wt => wt.WishList).
                Include(x => x.WishListItems).ThenInclude(wt => wt.WishList.User)
             .FirstOrDefault(x => x.Id == id);
            return cv;
        }

        public CvVM? EditedModelCv(int id)
        {
            CvVM? cvVm = _context.Cvs.Include(v => v.BusinessArea).
               Include(e => e.Education).
               Include(e => e.Experience).
               Include(c => c.City).
               Include(c => c.BusinessArea).ThenInclude(b => b.BusinessTitle).
               Include(o => o.OperatingMode).Select(p =>
                                          new CvVM
                                          {
                                              Id = p.Id,
                                              Name = p.Name,
                                              Surname = p.Surname,
                                              Email = p.Email,
                                              BusinessAreaId = p.BusinessAreaId,
                                              CityId = p.CityId,
                                              EducationId = p.EducationId,
                                              ExperienceId = p.ExperienceId,
                                              OperatingModeId = p.OperatingModeId,
                                              Salary = p.Salary,
                                              Position = p.Position,
                                              Number = p.Number,
                                              BornDate = p.BornDate,
                                              Images = p.Image,
                                              CvPDFs = p.CvPDF,
                                              Status = p.Status,
                                              Count = p.Count
                                          }).FirstOrDefault(x => x.Id == id);
            return cvVm;
        }

        public void CheckCv()
        {
            List<Cv> cv = _context.Cvs
                .Include(r => r.User)
                .ToList();

            foreach (var item in cv)
            {
                if (!item.TimeIsOver)
                {
                    if (item.EndedAt <= DateTime.Now)
                    {
                        MailMessage message = new MailMessage();
                        message.From = new MailAddress("hellojob440@gmail.com", "HelloJob");
                        message.To.Add(new MailAddress(item.User.Email));
                        message.Subject = "HelloJob";
                        message.Body = string.Empty;
                        message.Body = $"Last day of stickers {item.EndedAt.ToString("D")}" + " " +
                        $"{item.Name + " " + item.Surname + " " + item.Position + " from " + " link: https://localhost:7120/cvpage/detail?id=" + item.Id}";


                        SmtpClient smtpClient = new SmtpClient();
                        smtpClient.Host = "smtp.gmail.com";
                        smtpClient.Port = 587;
                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.Credentials = new NetworkCredential("hellojob440@gmail.com", "eomddhluuxosvnoy");
                        smtpClient.Send(message);
                        item.TimeIsOver = true;
                        _context.SaveChanges();
                    }
                }
            }
        }
    }

}
