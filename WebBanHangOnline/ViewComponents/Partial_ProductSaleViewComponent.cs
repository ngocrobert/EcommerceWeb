using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;

namespace WebBanHangOnline.ViewComponents
{
    public class Partial_ProductSaleViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public Partial_ProductSaleViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            var items = _db.Products.Where(x => x.IsSale && x.IsActive).Take(10).ToList();
            return View("Partial_ProductSale", items);
        }
    }
}
