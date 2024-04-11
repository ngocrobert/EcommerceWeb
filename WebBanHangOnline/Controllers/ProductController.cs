using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Index(int? id)
        {
            //var items = _db.Products.Include(p => p.ProductCategory).ToList();
            var items = _db.Products.ToList();
            if (id != null)
			{
                items = items.Where(x => x.Id == id).ToList();
			}
            return View(items);
        }
        public IActionResult ProductCategory(string alias, int id)
        {
            //var items = _db.Products.Include(p => p.ProductCategory).ToList();
            var items = _db.Products.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate = _db.ProductCategories.Find(id);
            if(cate!=null)
			{
                ViewBag.CateName = cate.Title;
			}
            ViewBag.CateId = id;
            return View(items);
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
        [Route("chi-tiet/{alias}-p{id}")]
        public IActionResult DetailProduct(string alias, int id)
        {
            var item = _db.Products.Find(id);
            //var item = _db.Products.Include(p => p.ProductCategory).Where(x => x.Id == id);
            item.ProductCategory = _db.ProductCategories.Find(item.ProductCategoryId);
            item.ProductImage = _db.ProductImages.Where(x => x.ProductId == id).ToList();
            return View(item);
        }
    }
}
