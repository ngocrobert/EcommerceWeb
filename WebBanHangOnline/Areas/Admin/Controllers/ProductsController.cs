using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;
using X.PagedList;
using WebBanHangOnline.Models.EF;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/products")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index(int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Product> items = _db.Products.OrderByDescending(x => x.Id);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        [Route("add")]
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.ProductCategory = new SelectList(_db.ProductCategories.ToList(), "Id", "Title");
            return View();
        }
    }
}
