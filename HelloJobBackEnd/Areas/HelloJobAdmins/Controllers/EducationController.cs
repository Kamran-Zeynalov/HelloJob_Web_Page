using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HelloJobBackEnd.Areas.HelloJobAdmins.Controllers
{
    [Authorize(Roles = "superadmin, admin, moderator")]
    [Area("HelloJobAdmins")]
    public class EducationController : Controller
    {
        private readonly HelloJobDbContext _context;

        public EducationController(HelloJobDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Education> educations = _context.Educations.OrderBy(x => x.Name).ToList();
            return View(educations);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            List<Education> educations = _context.Educations.OrderBy(x => x.Name).ToList();
            if (!string.IsNullOrEmpty(search))
            {
                educations = educations.Where(x => x.Name.ToLower().StartsWith(search.ToLower().Substring(0, Math.Min(search.Length, 3)))).ToList();
            }

            return View(educations);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Education neweducation)
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
            bool Isdublicate = _context.Educations.Any(c => c.Name == neweducation.Name);

            if (Isdublicate)
            {
                ModelState.AddModelError("", "You cannot enter the same data again");
                return View();
            }
            _context.Educations.Add(neweducation);
            _context.SaveChanges();
            TempData["Create"] = true;

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            Education? education = _context.Educations.FirstOrDefault(c => c.Id == id);
            if (education is null) return Redirect("~/Error/Error");
            return View(education);
        }

        [HttpPost]
        public IActionResult Edit(int id, Education editEducation)
        {
            TempData["Edit"] = false;

            if (id != editEducation.Id) return Redirect("~/Error/Error");
            Education? education = _context.Educations.FirstOrDefault(c => c.Id == id);
            if (education is null) return Redirect("~/Error/Error");
            bool duplicate = _context.Educations.Any(c => c.Name == editEducation.Name && education.Name != editEducation.Name);
            if (duplicate)
            {
                ModelState.AddModelError("Name", "This  Education name is now available");
                return View();
            }
            education.Name = editEducation.Name;
            _context.SaveChanges();
            TempData["Edit"] = true;

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            Education? education = _context.Educations.FirstOrDefault(c => c.Id == id);
            return education is null ? Redirect("~/Error/Error") : View(education);
        }



        public IActionResult Delete(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            Education? education = _context.Educations.FirstOrDefault(c => c.Id == id);
            if (education is null) return Redirect("~/Error/Error");
            return View(education);
        }

        [HttpPost]
        public IActionResult Delete(int id, Education deleteeducation)
        {
            TempData["Delete"] = false;

            if (id != deleteeducation.Id) return Redirect("~/Error/Error");
            Education? education = _context.Educations.FirstOrDefault(c => c.Id == id);
            if (education is null) return Redirect("~/Error/Error");
            _context.Educations.Remove(education);
            _context.SaveChanges();
            TempData["Delete"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
}
