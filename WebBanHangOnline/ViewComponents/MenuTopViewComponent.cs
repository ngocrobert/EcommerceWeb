using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebBanHangOnline.Data;
namespace WebBanHangOnline.ViewComponents
{
    public class MenuTopViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public MenuTopViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            var items = _db.Categories.OrderBy(x => x.Position).ToList();
            return View("_MenuTop", items);
        }
    }
}
