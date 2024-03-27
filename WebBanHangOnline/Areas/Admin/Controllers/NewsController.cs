using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Index()
        {
            var items = _db.News.OrderByDescending(x => x.Id).ToList();
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
                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
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
