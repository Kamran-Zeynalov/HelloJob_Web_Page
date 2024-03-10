using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Services;
using HelloJobBackEnd.Services.Interface;
using HelloJobBackEnd.Utilities.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;

namespace HelloJobBackEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly HelloJobDbContext _context;
        private readonly IVacansService _vacansService;
        private readonly ICompanyService _companyService;
        private readonly IBusinessTitleService _businessTitleService;
        private readonly ICvPageService _cvPageService;


        public HomeController(HelloJobDbContext context, IVacansService vacansService, ICompanyService companyService, IBusinessTitleService businessTitleService, ICvPageService cvPageService)
        {
            _vacansService = vacansService;
            _companyService = companyService;
            _businessTitleService = businessTitleService;
            _context = context;
            _cvPageService = cvPageService;
        }
        public IActionResult Index(int page = 1)
        {
            IQueryable<Vacans> allVacans = _vacansService.GetAcceptedVacansWithRelatedData();

            ViewBag.Company = _companyService.GetTopAcceptedCompaniesWithVacans(4).Where(x => x.Status == OrderStatus.Accepted).ToList();

            ViewBag.TotalPage = Math.Ceiling((double)_context.Vacans.Count() / 9);
            ViewBag.CurrentPage = page;

            List<Vacans> vacans = allVacans.OrderByDescending(x => x.Id).AsNoTracking().Skip((page - 1) * 9).Where(x => x.Status == OrderStatus.Accepted).Take(9).ToList();
            ViewBag.Titles = _businessTitleService.GetAllBusinessTitlesWithAreas();
            _vacansService.CheckVacans();
            _cvPageService.CheckCv();
            return View(vacans);

        }

        public IActionResult Search(string search)
        {
            IQueryable<Vacans> query = _vacansService.GetAcceptedVacansWithRelatedData()
                .Where(x => x.Position.Contains(search));

            List<Vacans> vacans = query.OrderByDescending(x => x.Id)
                .Take(3)
                .Where(c => c.Status == OrderStatus.Accepted)
                .ToList();

            return PartialView("_SearchvacansPartial", vacans);
        }

        public IActionResult Sorted(int titleid, int page = 1)
        {

            ViewBag.TotalPage = Math.Ceiling((double)_context.Vacans.Count() / 9);
            ViewBag.CurrentPage = page;
            IQueryable<Vacans> allVacans = _vacansService.GetAcceptedVacansWithRelatedData();

            List<Vacans> sortvacans = allVacans.OrderByDescending(x => x.Id).Where(x => x.BusinessArea.BusinessTitleId == titleid && x.Status == OrderStatus.Accepted).Skip((page - 1) * 9).ToList();

            return PartialView("_HomePartial", sortvacans);
        }



        public IActionResult SortedMode(int? modeid, int page = 1)
        {

            ViewBag.TotalPage = Math.Ceiling((double)_context.Vacans.Count() / 9);
            ViewBag.CurrentPage = page;
            if (modeid.HasValue)
            {
                IQueryable<Vacans> allVacans = _vacansService.GetAcceptedVacansWithRelatedData();
                List<Vacans> sortvacans = allVacans.OrderByDescending(x => x.Id).Where(x => x.OperatingModeId == modeid.Value && x.Status == OrderStatus.Accepted).Skip((page - 1) * 9).Take(9).ToList();
                return PartialView("_HomePartial", sortvacans);
            }
            else
            {
                return PartialView("_HomePartial", new List<Vacans>());
            }
        }

        public IActionResult SearchResult(string search, int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Vacans.Count() / 9);
            ViewBag.CurrentPage = page;
            IQueryable<Vacans> allVacans = _vacansService.GetAcceptedVacansWithRelatedData();

            if (search is not null)
            {
                allVacans = allVacans.Where(c => c.Position.Contains(search));
            }
            else
            {
                allVacans = allVacans;
            }
            List<Vacans> searching = allVacans.OrderByDescending(x => x.Id).Skip((page - 1) * 9).Where(x => x.Status == OrderStatus.Accepted).Take(9).ToList();

            return PartialView("_HomePartial", searching);

        }

        public ActionResult Subscribe(string email)
        {
            TempData["Subscribe"] = false;
            bool Isdublicate = _context.Subscribe.Any(c => c.Email == email);

            if (Isdublicate)
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            Subscribe subscribe = new()
            {
                Email = email
            };
            _context.Subscribe.Add(subscribe);
            _context.SaveChanges();
            TempData["Subscribe"] = true;
            return Redirect(Request.Headers["Referer"].ToString());
        }




    }



}
