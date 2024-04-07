﻿using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;

namespace WebBanHangOnline.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _db;
        public MenuController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Route("menu")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MenuTop()
        {
            var items = _db.Categories.OrderBy(x=>x.Position).ToList();
            return PartialView("_MenuTop", items);
        }
        public IActionResult MenuProductCategory()
        {
            var items = _db.ProductCategories.ToList();
            return PartialView("_MenuProductCategory", items);
        }
        public IActionResult MenuArrivals()
        {
            var items = _db.ProductCategories.ToList();
            return PartialView("_MenuArrivals", items);
        }
    }
}
