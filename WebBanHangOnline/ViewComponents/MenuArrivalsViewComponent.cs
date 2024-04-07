using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;

namespace WebBanHangOnline.ViewComponents
{
    public class MenuArrivalsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public MenuArrivalsViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            var items = _db.ProductCategories.ToList();
            return View("_MenuArrivals", items);
        }
    }
}
