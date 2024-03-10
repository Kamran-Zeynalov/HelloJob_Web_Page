using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HelloJobBackEnd.Controllers
{
    public class RulesController : Controller
    {
        private readonly HelloJobDbContext _context;

        public RulesController(HelloJobDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Rules> rules = _context.Rules.ToList();
            return View(rules);
        }
    }
}
