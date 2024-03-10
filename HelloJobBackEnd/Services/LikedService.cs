using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelloJobBackEnd.Services
{
    public class LikedService : ILikedService
    {
        private readonly UserManager<User> _userManager;
        private readonly HelloJobDbContext _context;

        public LikedService(UserManager<User> userManager, HelloJobDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<List<WishList>> GetWishLists(User user)
        {
            List<WishList> wishLists = await _context.WishLists
              .Include(x => x.User)
              .Include(x => x.WishListItems)
                  .ThenInclude(c => c.Cv)
                      .ThenInclude(v => v.OperatingMode)
              .Include(x => x.WishListItems)
                  .ThenInclude(c => c.Cv)
                      .ThenInclude(v => v.Experience)
              .Include(x => x.WishListItems)
                  .ThenInclude(c => c.Vacans)
                      .ThenInclude(v => v.Company)
              .Include(x => x.WishListItems)
                  .ThenInclude(c => c.Vacans)
                      .ThenInclude(v => v.BusinessArea)
                          .ThenInclude(ba => ba.BusinessTitle)
              .Where(w => w.UserId == user.Id)
              .ToListAsync();
            return wishLists;

        }

        public WishListItem CreateWishlistItem(string itemType, int itemId)
        {
            if (itemType == "Cv")
            {
                Cv? cv = _context.Cvs.FirstOrDefault(c => c.Id == itemId);
                if (cv is null)
                {
                    return null;
                }

                return new WishListItem
                {
                    CvId = cv.Id,
                    IsLiked = true
                };
            }
            else if (itemType == "Vacans")
            {
                Vacans? vacans = _context.Vacans.FirstOrDefault(v => v.Id == itemId);
                if (vacans is null)
                {
                    return null;
                }

                return new WishListItem
                {
                    VacansId = vacans.Id,
                    IsLiked = true
                };
            }

            return null;
        }

        public WishListItem GetWishlistItem(List<WishListItem> wishlistItems, string itemType, int itemId)
        {
            if (itemType == "Cv")
            {
                return wishlistItems.FirstOrDefault(item => item.CvId == itemId);
            }
            else if (itemType == "Vacans")
            {
                return wishlistItems.FirstOrDefault(item => item.VacansId == itemId);
            }

            return null;
        }
    }
}
