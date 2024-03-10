using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelloJobBackEnd.Services.Interface;

namespace HelloJobBackEnd.Controllers
{
    public class LikedController : Controller
    {
        private readonly UserManager<User> _usermanager;
        private readonly HelloJobDbContext _context;
        private readonly ICompanyService _companyService;
        private readonly ILikedService _likedService;

        public LikedController(UserManager<User> usermanager, HelloJobDbContext context, ICompanyService companyService, ILikedService likedService)
        {
            _usermanager = usermanager;
            _context = context;
            _companyService = companyService;
            _likedService = likedService;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = await _usermanager.FindByNameAsync(User.Identity.Name);

                if (user is null)
                {
                    return RedirectToAction("Index", "Home");
                }

                List<WishList> wishlists = await _likedService.GetWishLists(user);

                ViewBag.Setting = _context.Settings.ToDictionary(s => s.Key, s => s.Value);
                ViewBag.Company = _companyService.GetTopAcceptedCompaniesWithVacans(4);

                return View(wishlists);
            }
            ViewBag.Company = _companyService.GetTopAcceptedCompaniesWithVacans(4);
            return View();
        }


        //--------------------------------------------------------------------------------------------------
        public async Task<IActionResult> AddToWishlist(int itemId, string itemType)
        {
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);

            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }

            WishList wishlist = _context.WishLists
                .Include(w => w.WishListItems)
                .FirstOrDefault(w => w.UserId == user.Id);

            if (wishlist is null)
            {
                wishlist = new WishList
                {
                    UserId = user.Id,
                    User = user,
                    WishListItems = new List<WishListItem>()
                };

                _context.WishLists.Add(wishlist);
            }

            WishListItem existingItem = wishlist.WishListItems.FirstOrDefault(item => item.VacansId == itemId || item.CvId == itemId);
            if (existingItem != null)
            {
                return RedirectToAction("Index", "Home");
            }

            WishListItem wishlistItem = _likedService.CreateWishlistItem(itemType, itemId);
            if (wishlistItem is null)
            {
                return RedirectToAction("Index", "Home");
            }

            wishlist.WishListItems.Add(wishlistItem);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> RemoveFromWishlist(int itemId, string itemType)
        {
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);

            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }

            WishList? wishlist = _context.WishLists
                .Include(w => w.WishListItems)
                .FirstOrDefault(w => w.UserId == user.Id);

            if (wishlist is null)
            {
                return RedirectToAction("Index", "Home");
            }

            WishListItem wishlistItem = _likedService.GetWishlistItem(wishlist.WishListItems, itemType, itemId);
            if (wishlistItem != null)
            {
                wishlist.WishListItems.Remove(wishlistItem);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }





    }
}
