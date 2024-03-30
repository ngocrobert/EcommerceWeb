using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/productCategory")]
    public class ProductCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductCategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var items = _db.ProductCategories;
            return View(items);
        }
        [Route("add")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [Route("add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Filter.FilterChar(model.Title);
                _db.ProductCategories.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
