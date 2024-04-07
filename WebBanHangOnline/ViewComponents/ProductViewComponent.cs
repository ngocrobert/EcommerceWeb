using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;

namespace WebBanHangOnline.ViewComponents
{
	public class ProductViewComponent : ViewComponent
	{
        private readonly ApplicationDbContext _db;

        public ProductViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            var items = _db.Products.Where(x => x.IsHome && x.IsActive).Take(10).ToList();
            return View("Partial_ItemsByCateId", items);
        }
    }
}
