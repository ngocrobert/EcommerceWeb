using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;
using WebBanHangOnline.Extensions;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.ViewComponents
{
    public class Partial_Item_ThanhToanViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public Partial_Item_ThanhToanViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {

            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null)
            {
                return View("_Partial_Item_ThanhToan", cart.Items);
            }
            return View("_Partial_Item_ThanhToan");
        }
    }
}
