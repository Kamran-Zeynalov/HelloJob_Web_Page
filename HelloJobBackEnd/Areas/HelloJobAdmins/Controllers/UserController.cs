using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HelloJobBackEnd.Areas.HelloJobAdmins.Controllers
{
    [Authorize(Roles = "superadmin")]
    [Area("HelloJobAdmins")]
    public class UserController : Controller
    {
        private readonly HelloJobDbContext _context;
        private readonly UserService _userService;
        private readonly UserManager<User> _userManager;

        public UserController(HelloJobDbContext context, UserService userService, UserManager<User> userManager)
        {
            _context = context;
            _userService = userService;
            _userManager = userManager;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Users.Count() / 8);
            ViewBag.CurrentPage = page;
            List<User> userList = _userService.GetAllUsers().Skip((page - 1) * 8).Take(8).ToList();
            return View(userList);
        }

        [HttpPost]
        public IActionResult Index(string search, int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Users.Count() / 8);
            ViewBag.CurrentPage = page;
            List<User> userList = _userService.GetAllUsers().Skip((page - 1) * 8).Take(8).ToList();
            if (!string.IsNullOrEmpty(search))
            {
                userList = userList.Where(x => x.FullName.ToLower().StartsWith(search.ToLower().Substring(0, Math.Min(search.Length, 3)))).ToList();
            }

            return View(userList);
        }



        public async Task<IActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IQueryable<Cv> cvsToDelete = _context.Cvs.Where(cv => cv.UserId == id);
                _context.Cvs.RemoveRange(cvsToDelete);

                IQueryable<Company> companiesToDelete = _context.Companies.Where(company => company.UserId == id);
                _context.Companies.RemoveRange(companiesToDelete);

                IQueryable<Request> requestToDelete = _context.Requests.Where(rq => rq.UserId == id);
                _context.Requests.RemoveRange(requestToDelete);

                IQueryable<WishList> wishListsDelete = _context.WishLists.Where(ws => ws.UserId == id);
                _context.WishLists.RemoveRange(wishListsDelete);

                await _userManager.DeleteAsync(user);

                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(string id)
        {
            User user = _userService.GetUserById(id);
            return View(user);
        }

    }
}
