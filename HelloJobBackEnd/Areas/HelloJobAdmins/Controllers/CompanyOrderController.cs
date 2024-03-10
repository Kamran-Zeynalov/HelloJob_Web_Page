using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Utilities.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.Mail;
using System.Net;
using HelloJobBackEnd.Utilities.Extension;
using HelloJobBackEnd.ViewModel;
using Microsoft.AspNetCore.Identity;
using HelloJobBackEnd.Services.Interface;
using HelloJobBackEnd.Services;

namespace HelloJobBackEnd.Areas.HelloJobAdmins.Controllers
{
    [Authorize(Roles = "superadmin, admin, moderator")]
    [Area("HelloJobAdmins")]
    public class CompanyOrderController : Controller
    {
        private readonly HelloJobDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailService _emailService;
        private readonly ICompanyService _companyService;

        public CompanyOrderController(HelloJobDbContext context, IWebHostEnvironment env, IEmailService emailService, ICompanyService companyService)
        {
            _context = context;
            _env = env;
            _emailService = emailService;
            _companyService = companyService;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Companies.Count() / 8);
            ViewBag.CurrentPage = page;
            List<Company> companies = _context.Companies.Include(u => u.User).Include(v => v.Vacans).OrderByDescending(x => x.Id).AsNoTracking().Skip((page - 1) * 8).Take(8).ToList();
            return View(companies);
        }

        [HttpPost]
        public IActionResult Index(string search, int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Companies.Count() / 8);
            ViewBag.CurrentPage = page;
            List<Company> companies = _context.Companies.Include(u => u.User).Include(v => v.Vacans).OrderByDescending(x => x.Id).AsNoTracking().Skip((page - 1) * 8).Take(8).ToList();
            if (!string.IsNullOrEmpty(search))
            {
                companies = companies.Where(x => x.Name.ToLower().StartsWith(search.ToLower().Substring(0, Math.Min(search.Length, 3)))).ToList();
            }

            return View(companies);
        }



        public IActionResult Accept(int id)
        {
            TempData["CompanyAccepted"] = false;
            Company? company = _companyService.GetCompanyWithVacansById(id);

            if (company is null) return Redirect("~/Error/Error");
            company.Status = OrderStatus.Accepted;
            _context.SaveChanges();
            TempData["CompanyAccepted"] = true;
            string recipientEmail = company.User.Email;
            string body = string.Empty;
            string subject = "Elanla Bağlı Məlumat";

            string urlMessage = Url.Action("Detail", "Company", new { id = company.Id }, Request.Scheme);
            urlMessage = urlMessage.Replace("/HelloJobAdmins", "");

            using (StreamReader reader = new StreamReader("wwwroot/assets/template/StickyacceptedMail.html"))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{{userFullName}}", company.User.FullName);
            body = body.Replace("{{companyName}}", company.Name);
            body = body.Replace("{{position}}", company.Email);
            body = body.Replace("{{urlMessage}}", urlMessage);

            _emailService.SendEmail(recipientEmail, subject, body);

            return RedirectToAction("SendMail", new { urlMessage });
        }

        public IActionResult Reject(int id)
        {
            TempData["CompanyReject"] = false;
            Company? company = _companyService.GetCompanyWithVacansById(id);

            if (company is null) return Redirect("~/Error/Error");
            company.Status = OrderStatus.Rejected;
            _context.SaveChanges();
            TempData["CompanyReject"] = true;

            string recipientEmail = company.Email;
            string subject = "Elanla Bağlı Məlumat";
            string body = string.Empty;
            using (StreamReader reader = new("wwwroot/assets/template/StickyrejectedMail.html"))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{{userFullName}}", company.User.FullName);
            body = body.Replace("{{companyName}}", company.Name);

            _emailService.SendEmail(recipientEmail, subject, body);

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            TempData["Delete"] = false;
            Company? company = _companyService.GetCompanyWithVacansById(id);
            var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            string filepath = Path.Combine(imagefolderPath, "Company", company.Image);
            ExtensionMethods.DeleteImage(filepath);
            _context.Companies.Remove(company);
            _context.SaveChanges();
            TempData["Delete"] = true;
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> SendMail(string urlMessage)
        {
            List<Subscribe> subscribes = _context.Subscribe.ToList();
            foreach (Subscribe email in subscribes)
            {
                string recipientEmail = email.Email;
                string subject = "New Company";
                string body = $"Yeni Şirkət əlavə olundu: <br> {urlMessage}";

                _emailService.SendEmail(recipientEmail, subject, body);
            }

            return RedirectToAction("Index");
        }

    }
}
