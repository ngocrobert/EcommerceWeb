using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/productImage")]
    public class ProductImageController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductImageController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        [Route("Index/{id}")]
        public IActionResult Index(int id)
        {
            ViewBag.ProductId = id;
            var items = _db.ProductImages.Where(x=>x.ProductId == id).ToList();
            return View(items);
        }
        [Route("addImage")]
        [HttpPost]
        public IActionResult addImage(int productId, string url)
        {
            _db.ProductImages.Add(new ProductImage {
                ProductId = productId,
                Image = url,
                IsDefault = false
            });
            _db.SaveChanges();
            return Json(new { success = true });

        }
        [Route("delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _db.ProductImages.Find(id);
            if (item != null)
            {
                _db.ProductImages.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
