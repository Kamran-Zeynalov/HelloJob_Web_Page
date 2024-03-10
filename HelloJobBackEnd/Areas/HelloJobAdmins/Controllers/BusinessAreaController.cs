using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Utilities.Enum;
using HelloJobBackEnd.Utilities.Extension;
using HelloJobBackEnd.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HelloJobBackEnd.Areas.HelloJobAdmins.Controllers
{
    [Authorize(Roles = "superadmin, admin, moderator")]
    [Area("HelloJobAdmins")]
    public class BusinessAreaController : Controller
    {
        private readonly HelloJobDbContext _context;

        public BusinessAreaController(HelloJobDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.BusinessArea.Count() / 8);
            ViewBag.CurrentPage = page;
            IEnumerable<BusinessArea> areas = _context.BusinessArea.Include(b => b.BusinessTitle).OrderBy(b => b.BusinessTitleId).AsNoTracking().Skip((page - 1) * 8).Take(8).AsEnumerable();
            return View(areas);
        }
        [HttpPost]
        public IActionResult Index(string search, int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.BusinessArea.Count() / 8);
            ViewBag.CurrentPage = page;
            IEnumerable<BusinessArea> areas = _context.BusinessArea.Include(b => b.BusinessTitle).OrderBy(b => b.BusinessTitleId).AsNoTracking().Skip((page - 1) * 8).Take(8).AsEnumerable();
            if (!string.IsNullOrEmpty(search))
            {
                areas = areas.Where(x => x.Name.ToLower().StartsWith(search.ToLower().Substring(0, Math.Min(search.Length, 3)))).ToList();
            }

            return View(areas);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Business = _context.BusinessTitle.Include(b => b.BusinessAreas).ToList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BusinessArea newarea)
        {
            TempData["Create"] = false;
            ViewBag.Business = _context.BusinessTitle.Include(b => b.BusinessAreas).ToList();

            if (!ModelState.IsValid)
            {
                return View();
            }

            BusinessArea area = new()
            {
                Name = newarea.Name,
                BusinessTitleId = newarea.BusinessTitleId

            };
            _context.BusinessArea.Add(area);
            _context.SaveChanges();
            TempData["Create"] = true;

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            ViewBag.Business = _context.BusinessTitle.Include(b => b.BusinessAreas).ToList();
            BusinessArea areas = _context.BusinessArea.Include(b => b.BusinessTitle).FirstOrDefault(x => x.Id == id);
            if (areas is null) return Redirect("~/Error/Error");
            return View(areas);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BusinessArea edited)
        {
            TempData["Edit"] = false;
            if (id == 0) return Redirect("~/Error/Error");
            ViewBag.Business = _context.BusinessTitle.Include(b => b.BusinessAreas).ToList();
            BusinessArea areas = _context.BusinessArea.Include(b => b.BusinessTitle).FirstOrDefault(x => x.Id == id);

            areas.Name = edited.Name;
            areas.BusinessTitleId = edited.BusinessTitleId;
            _context.SaveChanges();
            TempData["Edit"] = false;
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id == 0) return NotFound();
            BusinessArea? area = _context.BusinessArea.Include(b => b.BusinessTitle).FirstOrDefault(s => s.Id == id);
            return area is null ? Redirect("~/Error/Error") : View(area);
        }

        public IActionResult Delete(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            BusinessArea? area = _context.BusinessArea.Include(x => x.BusinessTitle).FirstOrDefault(s => s.Id == id);
            if (area is null) return Redirect("~/Error/Error");
            return View(area);
        }

        [HttpPost]
        public IActionResult Delete(int id, BusinessArea deleted)
        {


            TempData["Delete"] = false;
            if (id != deleted.Id) return Redirect("~/Error/Error");
            BusinessArea? area = _context.BusinessArea.Include(x => x.BusinessTitle).FirstOrDefault(s => s.Id == id);
            if (area is null) return Redirect("~/Error/Error");
            _context.BusinessArea.Remove(area);
            _context.SaveChanges();
            TempData["Delete"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
}
