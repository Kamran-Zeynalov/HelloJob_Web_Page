using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Utilities.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.Mail;
using System.Net;
using HelloJobBackEnd.ViewModel;
using Microsoft.AspNetCore.Identity;
using HelloJobBackEnd.Services.Interface;
using HelloJobBackEnd.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HelloJobBackEnd.Areas.HelloJobAdmins.Controllers
{
    [Authorize(Roles = "superadmin, admin, moderator")]
    [Area("HelloJobAdmins")]
    public class VacansOrderController : Controller
    {

        private readonly HelloJobDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IVacansService _vacansService;

        public VacansOrderController(HelloJobDbContext context, IEmailService emailService, IVacansService vacansService)
        {
            _context = context;
            _emailService = emailService;
            _vacansService = vacansService;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Vacans.Count() / 8);
            ViewBag.CurrentPage = page;
            List<Vacans>? vacans = _vacansService.GetAcceptedVacansWithRelatedData().AsNoTracking().OrderByDescending(x => x.Id).Skip((page - 1) * 8).Take(8).ToList();
            return View(vacans);
        }

        [HttpPost]
        public IActionResult Index(string search, int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Vacans.Count() / 8);
            ViewBag.CurrentPage = page;
            List<Vacans>? vacans = _vacansService.GetAcceptedVacansWithRelatedData().AsNoTracking().OrderByDescending(x => x.Id).Skip((page - 1) * 8).Take(8).ToList();
            if (!string.IsNullOrEmpty(search))
            {
                vacans = vacans.Where(x => x.Position.ToLower().StartsWith(search.ToLower().Substring(0, Math.Min(search.Length, 3)))).ToList();
            }


            return View(vacans);
        }

        public IActionResult Accept(int id)
        {
            TempData["CompanyAccepted"] = false;
            Vacans? vacans = _vacansService.GetVacansWithRelatedEntitiesById(id);

            if (vacans is null) return Redirect("~/Error/Error");
            vacans.Status = OrderStatus.Accepted;
            _context.SaveChanges();
            TempData["CompanyAccepted"] = true;
            string recipientEmail = vacans.Company.User.Email;
            string subject = "Elanla Bağlı Məlumat";
            string body = string.Empty;
            string urlMessage = Url.Action("VacansDetail", "Company", new { id = vacans.Id }, Request.Scheme);
            urlMessage = urlMessage.Replace("/HelloJobAdmins", "");
            using (StreamReader reader = new StreamReader("wwwroot/assets/template/StickyacceptedMail.html"))
            {
                body = reader.ReadToEnd();
            }


            body = body.Replace("{{userFullName}}", vacans.Company.User.FullName);
            body = body.Replace("{{position}}", vacans.Position);
            body = body.Replace("{{companyName}}", vacans.Company.Name);
            body = body.Replace("{{urlMessage}}", urlMessage);

            _emailService.SendEmail(recipientEmail, subject, body);
            return RedirectToAction("SendMail", new { urlMessage });
        }
        public IActionResult Reject(int id)
        {
            TempData["CompanyReject"] = false;
            Vacans? vacans = _vacansService.GetVacansWithRelatedEntitiesById(id);

            if (vacans is null) return Redirect("~/Error/Error");
            vacans.Status = OrderStatus.Rejected;
            _context.SaveChanges();
            TempData["CompanyReject"] = true;
            string recipientEmail = vacans.Company.User.Email;
            string subject = "Elanla Bağlı Məlumat";
            string body = string.Empty;
            using (StreamReader reader = new StreamReader("wwwroot/assets/template/StickyrejectedMail.html"))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{{userFullName}}", vacans.Company.User.FullName);
            body = body.Replace("{{position}}", vacans.Position);
            body = body.Replace("{{companyName}}", vacans.Company.Name);

            _emailService.SendEmail(recipientEmail, subject, body);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            TempData["Deleted"] = false;
            Vacans? vacans = _vacansService.GetVacansWithRelatedEntitiesById(id);
            List<WishListItem> wishlistItems = _context.WishListItems.Where(w => w.VacansId == vacans.Id).ToList();
            List<RequestItem> requestItem = _context.RequestItems.Where(w => w.VacansId == vacans.Id).ToList();
            _context.WishListItems.RemoveRange(wishlistItems);
            _context.RequestItems.RemoveRange(requestItem);
            _context.Vacans.Remove(vacans);
            _context.SaveChanges();
            TempData["Deleted"] = true;
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            Vacans? vacans = _vacansService.GetVacansWithRelatedEntitiesById(id);
            return View(vacans);
        }
        public async Task<IActionResult> SendMail(string urlMessage)
        {
            List<Subscribe> subscribes = _context.Subscribe.ToList();
            foreach (Subscribe email in subscribes)
            {
                string recipientEmail = email.Email;
                string subject = "New Vacans";
                string body = $"Yeni VAcans əlavə olundu: <br> {urlMessage}";

                _emailService.SendEmail(recipientEmail, subject, body);
            }

            return RedirectToAction("Index");
        }

    }
}
