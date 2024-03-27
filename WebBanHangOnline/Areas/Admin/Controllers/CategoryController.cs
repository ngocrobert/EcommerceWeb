using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/category")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            var items = _db.Categories;
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
        public IActionResult Add(Category model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Filter.FilterChar(model.Title);
                _db.Categories.Add(model);
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(model);

        }
        [Route("edit")]
        [Route("edit/{id}")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _db.Categories.Find(id);
            return View(item);
        }
        [Route("edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Attach(model);
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Filter.FilterChar(model.Title);
                _db.Entry(model).Property(x => x.Title).IsModified = true;
                _db.Entry(model).Property(x => x.Description).IsModified = true;
                _db.Entry(model).Property(x => x.Alias).IsModified = true;
                _db.Entry(model).Property(x => x.SeoDescription).IsModified = true;
                _db.Entry(model).Property(x => x.SeoKeywords).IsModified = true;
                _db.Entry(model).Property(x => x.SeoTitle).IsModified = true;
                _db.Entry(model).Property(x => x.Position).IsModified = true;
                _db.Entry(model).Property(x => x.ModifiedDate).IsModified = true;
                _db.Entry(model).Property(x => x.ModifiedBy).IsModified = true;

                
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(model);

        }
        [Route("delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _db.Categories.Find(id);
            if (item != null)
            {
                _db.Categories.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new {success = false});
        }
    }
}
