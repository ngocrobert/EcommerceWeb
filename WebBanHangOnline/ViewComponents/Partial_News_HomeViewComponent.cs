using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;

namespace WebBanHangOnline.ViewComponents
{
    public class Partial_News_HomeViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public Partial_News_HomeViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            var items = _db.News.Take(3).ToList();
            return View("Partial_News_Home", items);
        }
    }
}
