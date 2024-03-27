using Microsoft.AspNetCore.Mvc;
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

    }
}
