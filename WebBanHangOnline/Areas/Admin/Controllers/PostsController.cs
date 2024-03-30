using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/posts")]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public PostsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var items = _db.Posts.ToList();
            return View(items);
        }
        [Route("add")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        /// <summary>
        /// Thêm 1 bài viết
        /// </summary>
        /// <param name="model">bài viết</param>
        /// <returns>bài viết</returns>
        [Route("add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Posts model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.CategoryId = 5;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Filter.FilterChar(model.Title);
                _db.Posts.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [Route("edit")]
        [Route("edit/{id}")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _db.Posts.Find(id);
            return View(item);
        }
        /// <summary>
        /// Sửa 1 bài viết
        /// </summary>
        /// <param name="model">bài viết</param>
        /// <returns>bài viết</returns>
        [Route("edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Posts model)
        {
            if (ModelState.IsValid)
            {

                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Filter.FilterChar(model.Title);
                _db.Posts.Attach(model);
                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        /// <summary>
        /// Xóa 1 bài viết
        /// </summary>
        /// <param name="id">id bài viết</param>
        /// <returns>1 đoạn JSON</returns>
        [Route("delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _db.Posts.Find(id);
            if (item != null)
            {
                _db.Posts.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }
        /// <summary>
        /// Chỉnh sửa trạng thái bài viết
        /// </summary>
        /// <param name="id">id bài viết</param>
        /// <returns>1 đoạn JSON</returns>
        [Route("IsActive")]
        [HttpPost]
        public IActionResult IsActive(int id)
        {
            var item = _db.Posts.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                _db.Entry(item).State = EntityState.Modified;
                _db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }
            return Json(new { success = false });

        }
        /// <summary>
        /// Xóa nhiều bài viết
        /// </summary>
        /// <param name="ids">danh sách Id bài viết</param>
        /// <returns>1 đoạn JSON</returns>
        [Route("deleteAll")]
        [HttpPost]
        public IActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = _db.Posts.Find(Convert.ToInt32(item));
                        _db.Posts.Remove(obj);
                        _db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

    }
}
