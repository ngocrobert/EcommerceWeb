using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _db;
        public RoleController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var items = _db.Roles.ToList();
            return View();
        }
    }
}
