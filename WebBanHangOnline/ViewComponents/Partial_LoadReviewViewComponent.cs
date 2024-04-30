using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models.EF;
using X.PagedList;

namespace WebBanHangOnline.ViewComponents
{
    public class Partial_LoadReviewViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public Partial_LoadReviewViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke(int productId)
        {
            
            var items = _db.Reviews.Where(x => x.ProductId == productId).OrderByDescending(x => x.Id).ToList().Take(3);
            ViewBag.Count = items.Count();
            return View("_LoadReview", items);
        }
    }
}
