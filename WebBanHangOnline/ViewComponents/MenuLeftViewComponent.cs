using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;

namespace WebBanHangOnline.ViewComponents
{
	public class MenuLeftViewComponent : ViewComponent
	{
        private readonly ApplicationDbContext _db;

        public MenuLeftViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            var items = _db.ProductCategories.ToList();
            return View("_MenuLeft", items);
        }
    }
}
