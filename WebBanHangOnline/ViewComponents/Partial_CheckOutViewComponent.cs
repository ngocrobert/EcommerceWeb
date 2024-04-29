using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;

namespace WebBanHangOnline.ViewComponents
{
    public class Partial_CheckOutViewComponent : ViewComponent
    {
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _db;

        public Partial_CheckOutViewComponent(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        //public Partial_CheckOutViewComponent(ApplicationDbContext db)
        //{
        //    _db = db;
        //}

        public IViewComponentResult Invoke()
        {

            //ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            //if (cart != null)
            //{
            //    return View("_Partial_CheckOut", cart.Items);
            //}
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            if (user != null)
            {
                ViewBag.User = user;
            }

            return View("_Partial_CheckOut");
        }
    }
}
