using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;
using WebBanHangOnline.Extensions;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.ViewComponents
{
    public class Partial_Item_CartViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public Partial_Item_CartViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {

            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null)
            {
                return View("_Partial_Item_Cart",cart.Items);
            }
            return View("_Partial_Item_Cart");
        }
    }
}
