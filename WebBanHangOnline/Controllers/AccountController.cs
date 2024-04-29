using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _db;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }
        public IActionResult Index()
        {
            var items = _db.Users.ToList();
            return View(items);

        }
        [Route("register")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Create()
        {
            //ViewBag.Role = new SelectList(_db.Roles.ToList(), "Id", "Name");
            return View();
        }

        //
        // POST: /Account/Register
        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    PhoneNumber = model.Phone
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Lấy vai trò được chọn từ form
                        // Thêm người dùng vào vai trò
                        await _userManager.AddToRoleAsync(user, "Customer");
                    

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }
            //ViewBag.Role = new SelectList(_db.Roles.ToList(), "Id", "Name");
            return View(model);
        }


        // GET: /Account/Login
        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            //ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                //return RedirectToLocal(returnUrl);
                return RedirectToAction("Index", "Home");
            }
            if (result.RequiresTwoFactor)
            {
                // Handle two-factor authentication
            }
            if (result.IsLockedOut)
            {
                // Handle account lockout
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            return View(model);
        }

        [Route("profile")]
        public  async Task<IActionResult> Profile()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var item = new CreateAccountViewModel();
            item.Email = user.Email;
            item.FullName = user.FullName;
            item.Phone = user.PhoneNumber;
            item.UserName = user.UserName;
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostProfile(CreateAccountViewModel req)
        {
            var user = await _userManager.FindByEmailAsync(req.Email);
            user.FullName = req.FullName;
            user.PhoneNumber = req.Phone;

            var rs = await _userManager.UpdateAsync(user);
                if(rs.Succeeded)
            {
                return RedirectToAction("Profile");
            }
                return View(req);
        }

        // POST: /Account/LogOff
        [Route("LogOff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
