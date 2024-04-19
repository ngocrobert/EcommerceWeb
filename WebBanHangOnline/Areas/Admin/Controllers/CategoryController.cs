using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/category")]
    [Authorize(Roles = "Admin")]
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
        /// <summary>
        /// Thêm 1 danh mục
        /// </summary>
        /// <param name="model">danh mục</param>
        /// <returns>danh mục</returns>
        [Route("add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Category model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                // thêm bí danh từ hàm có sẵn
                model.Alias = WebBanHangOnline.Models.Filter.FilterChar(model.Title);
                _db.Categories.Add(model);
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(model);

        }
        /// <summary>
        /// Sửa 1 danh mục
        /// </summary>
        /// <param name="id">id của danh mục</param>
        /// <returns>danh mục</returns>
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
        /// <summary>
        /// Xóa 1 danh mục
        /// </summary>
        /// <param name="id">id danh mục</param>
        /// <returns>1 đối tượng JSON</returns>
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
