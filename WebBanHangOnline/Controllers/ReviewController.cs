using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    [Authorize]
    [Route("Review")]
    public class ReviewController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _db;

        public ReviewController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> _Review(int productId)
        {
            ViewBag.ProductId = productId;
            var item = new ReviewProduct();
            if (User.Identity.IsAuthenticated)
            {

                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    item.Email = user.Email;
                    item.FullName = user.FullName;

                    return PartialView(item);

                }

            }
            return PartialView();
        }
        [AllowAnonymous]
        public IActionResult LoadReview(int productId)
        {
            var items = _db.Reviews.Where(x => x.ProductId == productId).OrderByDescending(x => x.Id).ToList();
            ViewBag.Count = items.Count;
            return PartialView("_LoadReview");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("PostReview")]
        public IActionResult PostReview(ReviewProduct req)
        {
            if(ModelState.IsValid)
            {
                req.CreatedDate = DateTime.Now;
                _db.Reviews.Add(req);
                _db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
