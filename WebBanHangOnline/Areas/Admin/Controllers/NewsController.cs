using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{

    [Area("admin")]
    [Route("admin/news")]
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public NewsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(string SearchText, int? page)
        {
            var pageSize = 5;
            if (page == null)
			{
                page = 1;
			}
            // lấy ra danh sách tin tức sắp xếp theo bài mới nhất theo Id
            IEnumerable<News> items = _db.News.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(SearchText))
            {
                // tìm kiếm tin tức theo bí danh hoặc tiêu đề
                items = items.Where(x => x.Alias.Contains(SearchText) || x.Title.Contains(SearchText));
            }
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
            return View();
        }
        /// <summary>
        /// Thêm 1 tin tức
        /// </summary>
        /// <param name="model">tin tức</param>
        /// <returns>tin tức</returns>
        [Route("add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(News model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.CategoryId = 5;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Filter.FilterChar(model.Title);
                _db.News.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        /// <summary>
        /// Sửa 1 tin tức
        /// </summary>
        /// <param name="id">Id tin tức</param>
        /// <returns>tin tức</returns>
        [Route("edit")]
        [Route("edit/{id}")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _db.News.Find(id);
            return View(item);
        }
        [Route("edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(News model)
        {
            if (ModelState.IsValid)
            {
                
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Filter.FilterChar(model.Title);
                _db.News.Attach(model);
                // đánh dấu model đã được sửa => update CSDL
                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        /// <summary>
        /// Xóa 1 tin tức
        /// </summary>
        /// <param name="id">Id tin tức</param>
        /// <returns>1 đối tượng JSON</returns>
		[Route("delete")]
		[HttpPost]
        public IActionResult Delete(int id)
		{
            var item = _db.News.Find(id);
            if (item != null)
			{
                _db.News.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
			}
            return Json(new { success = false });

        }
        /// <summary>
        /// Chỉnh sửa trạng thái tin tức
        /// </summary>
        /// <param name="id">Id tin tức</param>
        /// <returns>1 đối tượng JSON</returns>
        [Route("IsActive")]
        [HttpPost]
        public IActionResult IsActive(int id)
        {
            var item = _db.News.Find(id);
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
        /// Xóa nhiều tin tức
        /// </summary>
        /// <param name="ids">danh sách Id tin tức</param>
        /// <returns>1 đối tượng JSON</returns>
		[Route("deleteAll")]
		[HttpPost]
        public IActionResult DeleteAll(string ids)
		{
			if (!string.IsNullOrEmpty(ids))
			{
                var items = ids.Split(',');
                if(items!=null && items.Any())
				{
                    foreach (var item in items)
					{
                        var obj = _db.News.Find(Convert.ToInt32(item));
                        _db.News.Remove(obj);
                        _db.SaveChanges();
                    }
				}
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
