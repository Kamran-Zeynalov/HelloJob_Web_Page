using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Configuration;

namespace HelloJobBackEnd.Areas.HelloJobAdmins.Controllers
{
    [Authorize(Roles = "superadmin, admin, moderator")]
    [Area("HelloJobAdmins")]
    public class SubscribeController : Controller
    {
        private readonly HelloJobDbContext _context;

        public SubscribeController(HelloJobDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Subscribe.Count() / 8);
            ViewBag.CurrentPage = page;
            List<Subscribe> subscribes = _context.Subscribe.Skip((page - 1) * 8).Take(8).ToList();
            return View(subscribes);
        }

        [HttpPost]
        public IActionResult Index(string search, int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Subscribe.Count() / 8);
            ViewBag.CurrentPage = page;
            List<Subscribe> subscribes = _context.Subscribe.Skip((page - 1) * 8).Take(8).ToList();
            if (!string.IsNullOrEmpty(search))
            {
                subscribes = subscribes.Where(x => x.Email.ToLower().StartsWith(search.ToLower().Substring(0, Math.Min(search.Length, 2)))).ToList();
            }
            return View(subscribes);
        }

        public IActionResult Delete(int id)
        {
            TempData["Delete"] = false;

            if (id == 0) return Redirect("~/Error/Error");


            Subscribe? subscribe = _context.Subscribe.FirstOrDefault(x => x.Id == id);

            if (subscribe is null) return Redirect("~/Error/Error");

            _context.Subscribe.Remove(subscribe);
            _context.SaveChanges();
            TempData["Delete"] = true;
            return RedirectToAction(nameof(Index));
        }

    }
}
