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
    public class VacansService : IVacansService
    {
        private readonly HelloJobDbContext _context;

        public VacansService(HelloJobDbContext context)
        {
            _context = context;
        }

        public IQueryable<Vacans> GetAcceptedVacansWithRelatedData()
        {
            IQueryable<Vacans> allVacans = _context.Vacans.Include(v => v.BusinessArea).
              Include(e => e.Education).
              Include(e => e.Experience).
              Include(o => o.OperatingMode).
              Include(o => o.InfoWorks).
              Include(o => o.infoEmployeers).
              Include(c => c.City).
              Include(c => c.Company).
              Include(c => c.Company).
                ThenInclude(x => x.User).
                Include(c => c.BusinessArea).ThenInclude(b => b.BusinessTitle).
                     Include(x => x.WishListItems).ThenInclude(wt => wt.WishList).
               Include(x => x.WishListItems).ThenInclude(wt => wt.WishList.User).AsNoTracking();
            return allVacans;
        }

        public Vacans? GetVacansWithRelatedEntitiesById(int id)
        {
            Vacans? vacans = _context.Vacans.Include(v => v.BusinessArea).
              Include(e => e.Education).
              Include(e => e.Experience).
              Include(o => o.OperatingMode).
              Include(o => o.InfoWorks).
              Include(o => o.infoEmployeers).
              Include(c => c.City).
              Include(c => c.Company).
              Include(c => c.Company).
                ThenInclude(x => x.User).
                Include(c => c.BusinessArea).ThenInclude(b => b.BusinessTitle).
                     Include(x => x.WishListItems).ThenInclude(wt => wt.WishList).
               Include(x => x.WishListItems).ThenInclude(wt => wt.WishList.User)
               .FirstOrDefault(x => x.Id == id);

            return vacans;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public void AddInfoWorks(Vacans vacans, string workInfo)
        {
            string[] workInfoArray = workInfo?.Split('/') ?? new string[0];
            foreach (string info in workInfoArray)
            {
                InfoWork infoWork = new InfoWork
                {
                    Vacans = vacans,
                    Info = info
                };

                _context.InfoWorks.Add(infoWork);
            }
        }

        public void AddInfoEmployeers(Vacans vacans, string employeeInfo)
        {
            string[] employeeInfoArray = employeeInfo?.Split('/') ?? new string[0];
            foreach (string info in employeeInfoArray)
            {
                InfoEmployeer infoEmployeer = new InfoEmployeer
                {
                    Vacans = vacans,
                    Info = info
                };

                _context.InfoEmployeers.Add(infoEmployeer);
            }
        }


        public VacansVM? GetEditedModelVC(int id)
        {
            VacansVM? vacansVM = _context.Vacans
                .Include(v => v.BusinessArea)
                .Include(e => e.Education)
                .Include(e => e.Experience)
                .Include(c => c.City)
                .Include(c => c.Company)
                .Include(c => c.BusinessArea)
                .ThenInclude(b => b.BusinessTitle)
                .Include(i => i.infoEmployeers)
                .Include(i => i.InfoWorks)
                .Include(o => o.OperatingMode)
                .Select(p => new VacansVM
                {
                    Id = p.Id,
                    BusinessAreaId = p.BusinessAreaId,
                    CityId = p.CityId,
                    EducationId = p.EducationId,
                    ExperienceId = p.ExperienceId,
                    OperatingModeId = p.OperatingModeId,
                    CompanyId = p.CompanyId,
                    Salary = p.Salary,
                    Position = p.Position,
                    AllEmployeerInfos = p.infoEmployeers,
                    AllWorkInfos = p.InfoWorks,
                    Status = p.Status
                })
                .FirstOrDefault(x => x.Id == id);

            return vacansVM;
        }

        public void CheckVacans()
        {
            List<Vacans> vacans = _context.Vacans
                .Include(r => r.Company).ThenInclude(x => x.User)
                .ToList();

            foreach (var item in vacans)
            {
                if (!item.TimeIsOver)
                {
                    if (item.EndedAt <= DateTime.Now)
                    {
                        MailMessage message = new MailMessage();
                        message.From = new MailAddress("hellojob440@gmail.com", "HelloJob");
                        message.To.Add(new MailAddress(item.Company.User.Email));
                        message.Subject = "HelloJob";
                        message.Body = string.Empty;
                        message.Body = $"Last day of stickers {item.EndedAt.ToString("D")}" + " " +
                        $"{item.Company.Name + " " + item.Position + " from " + " link: https://localhost:7120/company/vacansdetail?id=" + item.Id}";


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


