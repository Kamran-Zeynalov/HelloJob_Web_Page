using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Utilities.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HelloJobBackEnd.Areas.HelloJobAdmins.Controllers
{
    [Authorize(Roles = "superadmin, admin, moderator")]
    [Area("HelloJobAdmins")]
    public class SettingController : Controller
    {
        private readonly HelloJobDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SettingController(HelloJobDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Settings.Count() / 8);
            ViewBag.CurrentPage = page;
            IEnumerable<Setting> settings = _context.Settings.AsNoTracking().Skip((page - 1) * 8).Take(8).AsEnumerable();
            return View(settings);
        }


        [HttpPost]
        public IActionResult Index(string search, int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Settings.Count() / 8);
            ViewBag.CurrentPage = page;
            IEnumerable<Setting> settings = _context.Settings.AsNoTracking().Skip((page - 1) * 8).Take(8).AsEnumerable();
            if (!string.IsNullOrEmpty(search))
            {
                settings = settings.Where(x => x.Key.ToLower().StartsWith(search.ToLower().Substring(0, Math.Min(search.Length, 1)))).ToList();
            }

            return View(settings);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Setting newSetting)
        {
            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }
                return View();
            }

            if (newSetting.Image != null && newSetting.Image.Length > 0)
            {
                var imagefolderPath = Path.Combine(_env.WebRootPath, "assets");
                newSetting.Value = await newSetting.Image.CreateImage(imagefolderPath, "images");
            }

            bool IsDuplicate = _context.Settings.Any(c => c.Key == newSetting.Key);

            if (IsDuplicate)
            {
                ModelState.AddModelError("", "You cannot enter the same data again");
                return View();
            }

            _context.Settings.Add(newSetting);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }



        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return Redirect("~/Error/Error");
            }
            Setting setting = _context.Settings.FirstOrDefault(s => s.Id == id);
            if (setting is null)
            {
                return Redirect("~/Error/Error");
            }
            return View(setting);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Setting edited)
        {
            if (id != edited.Id) return Redirect("~/Error/Error");
            Setting setting = _context.Settings.FirstOrDefault(s => s.Id == id);
            if (!ModelState.IsValid) return View(setting);
            _context.Entry<Setting>(setting).CurrentValues.SetValues(edited);

            if (edited.Image is not null)
            {
                var imagefolderPath = Path.Combine(_env.WebRootPath, "assets");

                string filepath = Path.Combine(imagefolderPath, "images", setting.Value);
                ExtensionMethods.DeleteImage(filepath);
                setting.Value = await edited.Image.CreateImage(imagefolderPath, "images");
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Details(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            Setting? setting = _context.Settings.FirstOrDefault(c => c.Id == id);
            return setting is null ? Redirect("~/Error/Error") : View(setting);
        }



        public IActionResult Delete(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            Setting? setting = _context.Settings.FirstOrDefault(c => c.Id == id);
            if (setting is null) return Redirect("~/Error/Error");
            return View(setting);
        }

        [HttpPost]
        public IActionResult Delete(int id, Setting deleted)
        {
            if (id != deleted.Id) return Redirect("~/Error/Error");
            Setting? setting = _context.Settings.FirstOrDefault(c => c.Id == id);
            if (setting is null) return Redirect("~/Error/Error");
            _context.Settings.Remove(setting);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
