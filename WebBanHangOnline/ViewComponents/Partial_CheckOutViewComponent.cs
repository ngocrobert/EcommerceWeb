using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;

namespace WebBanHangOnline.ViewComponents
{
    public class Partial_CheckOutViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public Partial_CheckOutViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {

            //ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            //if (cart != null)
            //{
            //    return View("_Partial_CheckOut", cart.Items);
            //}
            return View("_Partial_CheckOut");
        }
    }
}
