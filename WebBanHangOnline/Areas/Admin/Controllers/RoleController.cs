using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/role")]
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller 
    {
        private readonly ApplicationDbContext _db;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> _roleManager;

        public RoleController(ApplicationDbContext db, Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;


        }
        public IActionResult Index()
        {
            var items = _db.Roles.ToList();
            return View(items);
        }
        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));

                var roleExist = await _roleManager.RoleExistsAsync(model.Name);
                if (!roleExist)
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Role");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Role already exists.");
                }
            }
            return View(model);
        }
        [Route("edit/{id}")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _db.Roles.Find(id);
            return View(item);
        }

        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));

                var roleExist = await _roleManager.RoleExistsAsync(model.Name);
                if (!roleExist)
                {
                    var result = await _roleManager.UpdateAsync(new IdentityRole(model.Name));
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Role");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Role already exists.");
                }
            }
            return View(model);
        }
    }
}
