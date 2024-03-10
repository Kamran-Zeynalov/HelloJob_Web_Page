using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using HelloJobBackEnd.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HelloJobBackEnd.Areas.HelloJobAdmins.Controllers
{
    [Authorize(Roles = "superadmin, admin, moderator")]
    [Area("HelloJobAdmins")]
    public class CityController : Controller
    {
        private readonly HelloJobDbContext _context;

        public CityController(HelloJobDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Cities.Count() / 8);
            ViewBag.CurrentPage = page;
            List<City> cities = _context.Cities.OrderBy(x => x.Name).AsNoTracking().Skip((page - 1) * 8).Take(8).ToList();
            return View(cities);
        }

        [HttpPost]
        public IActionResult Index(string search, int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Cities.Count() / 8);
            ViewBag.CurrentPage = page;
            List<City> cities = _context.Cities.OrderBy(x => x.Name).AsNoTracking().Skip((page - 1) * 8).Take(8).ToList();
            if (!string.IsNullOrEmpty(search))
            {
                cities = cities.Where(x => x.Name.ToLower().StartsWith(search.ToLower().Substring(0, Math.Min(search.Length, 3)))).ToList();
            }
            return View(cities);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(City newcity)
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
            bool Isdublicate = _context.Cities.Any(c => c.Name == newcity.Name);

            if (Isdublicate)
            {
                ModelState.AddModelError("", "You cannot enter the same data again");
                return View();
            }
            _context.Cities.Add(newcity);
            _context.SaveChanges();
            TempData["Create"] = true;

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            City? city = _context.Cities.FirstOrDefault(c => c.Id == id);
            if (city is null) return Redirect("~/Error/Error");
            return View(city);
        }

        [HttpPost]
        public IActionResult Edit(int id, City editCity)
        {
            TempData["Edit"] = false;

            if (id != editCity.Id) return Redirect("~/Error/Error");
            City? city = _context.Cities.FirstOrDefault(c => c.Id == id);
            if (city is null) return Redirect("~/Error/Error");
            bool duplicate = _context.Cities.Any(c => c.Name == editCity.Name && city.Name != editCity.Name);
            if (duplicate)
            {
                ModelState.AddModelError("Name", "This  city name is now available");
                return View();
            }
            city.Name = editCity.Name;
            _context.SaveChanges();
            TempData["Edit"] = true;

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            City? city = _context.Cities.FirstOrDefault(c => c.Id == id);
            return city is null ? Redirect("~/Error/Error") : View(city);
        }



        public IActionResult Delete(int id)
        {
            if (id == 0) return Redirect("~/Error/Error");
            City? city = _context.Cities.FirstOrDefault(c => c.Id == id);
            if (city is null) return Redirect("~/Error/Error");
            return View(city);
        }

        [HttpPost]
        public IActionResult Delete(int id, City deleteCity)
        {
            TempData["Delete"] = false;

            if (id != deleteCity.Id) return Redirect("~/Error/Error");
            City? city = _context.Cities.FirstOrDefault(c => c.Id == id);
            if (city is null) return Redirect("~/Error/Error");
            _context.Cities.Remove(city);
            _context.SaveChanges();
            TempData["Delete"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
}
