using HelloJobBackEnd.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HelloJobBackEnd.Services.Interface
{
    public interface ILikedService
    {
        Task<List<WishList>> GetWishLists(User user);
        WishListItem CreateWishlistItem(string itemType, int itemId);
        WishListItem GetWishlistItem(List<WishListItem> wishlistItems, string itemType, int itemId);

    }
}
