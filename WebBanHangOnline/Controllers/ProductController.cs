using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;

namespace WebBanHangOnline.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Partial_ItemsByCateId()
        {
            var items = _db.Products.Where(x=>x.IsHome && x.IsActive).Take(10).ToList();
            return PartialView("Partial_ItemsByCateId", items);
        }
        public IActionResult Partial_ProductSale()
        {
            var items = _db.Products.Where(x => x.IsSale && x.IsActive).Take(10).ToList();
            return PartialView("Partial_ProductSale", items);
        }

    }
}
