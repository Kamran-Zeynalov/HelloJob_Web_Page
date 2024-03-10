using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HelloJobBackEnd.Areas.HelloJobAdmins.Controllers
{
    [Authorize(Roles = "superadmin, admin, moderator")]
    [Area("HelloJobAdmins")]
    public class ExperienceController : Controller
    {
        private readonly HelloJobDbContext _context;

        public ExperienceController(HelloJobDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Experience> experiences = _context.Experiences.OrderBy(x => x.Name).ToList();
            return View(experiences);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            List<Experience> experiences = _context.Experiences.OrderBy(x => x.Name).ToList();
            if (!string.IsNullOrEmpty(search))
            {
                experiences = experiences.Where(x => x.Name.ToLower().StartsWith(search.ToLower().Substring(0, Math.Min(search.Length, 3)))).ToList();
            }

            return View(experiences);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Experience newexperience)
        {
            TempData["Create"] = false;
            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }
                return View();
            }
            bool Isdublicate = _context.Experiences.Any(c => c.Name == newexperience.Name);

            if (Isdublicate)
            {
                ModelState.AddModelError("", "You cannot enter the same data again");
                return View();
            }
            _context.Experiences.Add(newexperience);
            _context.SaveChanges();
            TempData["Create"] = true;

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            Experience? experience = _context.Experiences.FirstOrDefault(c => c.Id == id);
            if (experience is null) return Redirect("~/Error/Error");
            return View(experience);
        }

        [HttpPost]
        public IActionResult Edit(int id, Experience editexperience)
        {
            TempData["Edit"] = false;

            if (id != editexperience.Id) return Redirect("~/Error/Error");
            Experience? experience = _context.Experiences.FirstOrDefault(c => c.Id == id);
            if (experience is null) return Redirect("~/Error/Error");
            bool duplicate = _context.Experiences.Any(c => c.Name == editexperience.Name && experience.Name != editexperience.Name);
            if (duplicate)
            {
                ModelState.AddModelError("Name", "This  Experience name is now available");
                return View();
            }
            experience.Name = editexperience.Name;
            _context.SaveChanges();
            TempData["Edit"] = true;

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            Experience? experience = _context.Experiences.FirstOrDefault(c => c.Id == id);
            return experience is null ? Redirect("~/Error/Error") : View(experience);
        }



        public IActionResult Delete(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            Experience? experience = _context.Experiences.FirstOrDefault(c => c.Id == id);
            if (experience is null) return Redirect("~/Error/Error");
            return View(experience);
        }

        [HttpPost]
        public IActionResult Delete(int id, Experience deleteexperience)
        {
            TempData["Delete"] = false;

            if (id != deleteexperience.Id) return Redirect("~/Error/Error");
            Experience? experience = _context.Experiences.FirstOrDefault(c => c.Id == id);
            if (experience is null) return Redirect("~/Error/Error");
            _context.Experiences.Remove(experience);
            _context.SaveChanges();
            TempData["Delete"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
}
