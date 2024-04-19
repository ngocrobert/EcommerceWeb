using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    //[Authorize]
    [Authorize(Roles = "Admin, Employee")]
    public class HomeAdminController : Controller
    {
        [Route("")]
        [Route("index")]
        [Route("home")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
