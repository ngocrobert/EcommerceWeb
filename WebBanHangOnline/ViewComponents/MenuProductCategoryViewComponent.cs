using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;

namespace WebBanHangOnline.ViewComponents
{
    public class MenuProductCategoryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public MenuProductCategoryViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            var items = _db.ProductCategories.ToList();
            return View("_MenuProductCategory", items);
        }
    }
}
