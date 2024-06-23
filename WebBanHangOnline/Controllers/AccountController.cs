using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
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
        public IConfiguration Configuration { get; set; }

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db, IConfiguration _configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
            Configuration = _configuration;
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

        [Route("forgotPassword")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Route("forgotPassword")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    ModelState.AddModelError(string.Empty, "Email không hợp lệ hoặc không tồn tại.");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771

                // Send an email with this link
                string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                this.SendHtmlFormattedEmail(model.Email,"Quên mật khẩu", "Bạn click vào <a href='"+callbackUrl+"'>link này</a> để reset mật khẩu");
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Route("ForgotPasswordConfirmation")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ResetPassword
        [Route("ResetPassword")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [Route("ResetPassword")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [Route("ResetPasswordConfirmation")]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
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

        private void SendHtmlFormattedEmail(string recepEmail, string subject, string body)
        {
            string host = this.Configuration.GetValue<string>("Smtp:Server");
            int port = this.Configuration.GetValue<int>("Smtp:Port");
            string fromAddress = this.Configuration.GetValue<string>("Smtp:FromAddress");
            string userName = this.Configuration.GetValue<string>("Smtp:Username");
            string password = this.Configuration.GetValue<string>("Smtp:Password");
            using (MailMessage mm = new MailMessage(fromAddress, recepEmail))
            {
                MailAddress fromEmail = new MailAddress(fromAddress, "ShopOnline");
                mm.From = fromEmail;
                mm.Subject = subject;
                mm.Body = body;
                mm.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = host;
                    smtp.EnableSsl = true;
                    NetworkCredential networkCred = new NetworkCredential(userName, password);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = networkCred;
                    smtp.Port = port;
                    smtp.Send(mm);
                }
            }
        }
    }
}
