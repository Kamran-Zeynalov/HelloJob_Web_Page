using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Services.Interface;
using HelloJobBackEnd.Utilities.Enum;
using HelloJobBackEnd.Utilities.Extension;
using HelloJobBackEnd.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HelloJobBackEnd.Areas.HelloJobAdmins.Controllers
{
    [Authorize(Roles = "superadmin, admin, moderator")]
    [Area("HelloJobAdmins")]
    public class BusinessTitleController : Controller
    {
        private readonly HelloJobDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IBusinessTitleService _businessTitleService;

        public BusinessTitleController(HelloJobDbContext context, IWebHostEnvironment env, IBusinessTitleService businessTitleService)
        {
            _context = context;
            _env = env;
            _businessTitleService = businessTitleService;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.BusinessTitle.Count() / 4);
            ViewBag.CurrentPage = page;
            IEnumerable<BusinessTitle> businessTitles = _context.BusinessTitle.Include(b => b.BusinessAreas).AsNoTracking().Skip((page - 1) * 4).Take(4).AsEnumerable();
            return View(businessTitles);
        }


        [HttpPost]
        public IActionResult Index(string search, int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.BusinessTitle.Count() / 4);
            ViewBag.CurrentPage = page;
            IEnumerable<BusinessTitle> businessTitles = _context.BusinessTitle.Include(b => b.BusinessAreas).AsNoTracking().Skip((page - 1) * 4).Take(4).AsEnumerable();
            if (!string.IsNullOrEmpty(search))
            {
                businessTitles = businessTitles.Where(x => x.Name.ToLower().StartsWith(search.ToLower().Substring(0, Math.Min(search.Length, 3)))).ToList();
            }

            return View(businessTitles);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BusinessTitleVM newtitle)
        {
            TempData["Create"] = false;
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!newtitle.Image.IsValidFile("image/"))
            {
                ModelState.AddModelError(string.Empty, "Şəkil file seçin");
                return View();
            }
            if (!newtitle.Image.IsValidLength(1))
            {
                ModelState.AddModelError(string.Empty, "Maximum ölçü 1 mb ola bilər");
                return View();
            }
            BusinessTitle title = new()
            {
                Name = newtitle.Name,
            };

            var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            title.Image = await newtitle.Image.CreateImage(imagefolderPath, "BusinessTitle");

            _context.BusinessTitle.Add(title);
            _context.SaveChanges();
            TempData["Create"] = true;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            BusinessTitleVM? titleVM = _businessTitleService.EditedTItle(id);
            if (titleVM is null) return Redirect("~/Error/Error");
            return View(titleVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, BusinessTitleVM edited)
        {
            TempData["Edit"] = false;
            BusinessTitleVM? titleVM = _businessTitleService.EditedTItle(id);
            BusinessTitle title = _businessTitleService.GetTitleById(id);
            if (edited.Image is not null)
            {
                if (!edited.Image.IsValidFile("image/"))
                {
                    ModelState.AddModelError(string.Empty, "Şəkil file seçin");
                    return View();
                }
                if (!edited.Image.IsValidLength(2))
                {
                    ModelState.AddModelError(string.Empty, "Maximum ölçü 1 mb ola bilər");
                    return View();
                }
                var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
                string filepath = Path.Combine(imagefolderPath, "BusinessTitle", title.Image);
                ExtensionMethods.DeleteImage(filepath);
                title.Image = await edited.Image.CreateImage(imagefolderPath, "BusinessTitle");
            }

            title.Name = edited.Name;
            _context.SaveChanges();
            TempData["Edit"] = true;
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Details(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            BusinessTitle? title = _businessTitleService.GetTitleById(id);
            return title is null ? Redirect("~/Error/Error") : View(title);
        }

        public IActionResult Delete(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            BusinessTitle? title = _businessTitleService.GetTitleById(id);
            if (title is null) return Redirect("~/Error/Error");
            return View(title);
        }
        [HttpPost]
        public IActionResult Delete(int id, BusinessTitle deleted)
        {
            TempData["Delete"] = false;

            if (id != deleted.Id)
                return Redirect("~/Error/Error");

            BusinessTitle? title = _businessTitleService.GetTitleById(id);

            if (title is null)
                return Redirect("~/Error/Error");

            string imageFolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            string filePath = Path.Combine(imageFolderPath, "BusinessTitle", title.Image);

            ExtensionMethods.DeleteImage(filePath);

            _context.BusinessArea.RemoveRange(title.BusinessAreas);
            _context.BusinessTitle.Remove(title);
            _context.SaveChanges();

            TempData["Delete"] = true;
            return RedirectToAction(nameof(Index));
        }


    }
}
