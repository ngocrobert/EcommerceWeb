using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        [Route("Home")]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Partial_Subcribe()
        {
            return PartialView();
        }
        [Route("Subscribe")]
        [HttpPost]
        
        public IActionResult Subscribe(Subscribe req)
        {
            if (ModelState.IsValid)
            {
                _db.Subscribes.Add(new Subscribe { Email = req.Email, CreatedDate = DateTime.Now });
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}