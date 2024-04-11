using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;
using WebBanHangOnline.Extensions;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ShoppingCartController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null)
            {
                return View(cart.Items);
            }
            return View();
        }
        [HttpGet]

        public IActionResult ShowCount()
        {
            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null)
            {
                return Json(new { count = cart.Items.Count });
            }

            return Json(new { count = 0 });
        }

        [HttpPost]
        public IActionResult AddToCart(int id, int quantity)
        {
            var code = new { success = false, msg = "", code = -1, count = 0 };
            var checkProduct = _db.Products.FirstOrDefault(x => x.Id == id);
            checkProduct.ProductCategory = _db.ProductCategories.Find(checkProduct.ProductCategoryId);
            checkProduct.ProductImage = _db.ProductImages.Where(x => x.ProductId == id).ToList();
            if (checkProduct != null)
            {
                ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    Alias = checkProduct.Alias,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Quantity = quantity

                };
                if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
                }
                item.Price = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    item.Price = (decimal)checkProduct.PriceSale;
                }
                item.TotalPrice = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                HttpContext.Session.Set("Cart", cart);
                code = new { success = true, msg = "Thêm sp vào giỏ thành công", code = 1, count = cart.Items.Count };
            }


            return Json(code);
        }
        
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var code = new { success = false, msg = "", code = -1, count = 0 };

            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if(checkProduct!=null)
                {
                    cart.Remove(id);
                    HttpContext.Session.Set("Cart", cart);
                    code = new { success = true, msg = "", code = 1, count = cart.Items.Count };
                }
            }

            return Json(code);
        }
    }
}
