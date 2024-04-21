using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models.EF;
using X.PagedList;

namespace WebBanHangOnline.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public NewsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(int? page)
        {
            var pageSize = 5;
            if(page==null)
            {
                page = 1;
            }
            IEnumerable<News> items = _db.News.OrderByDescending(x=>x.CreatedDate).ToList();
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        
        public IActionResult Detail(int id)
        {
            var item = _db.News.Find(id);
            return View(item);
        }

        public IActionResult Partial_News_Home()
        {
            //var items = _db.News.Take(3).ToList();
            return PartialView();
        }
    }
}
