using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;
using WebBanHangOnline.Extensions;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

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
            //ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            //if (cart != null)
            //{
            //    return View(cart.Items);
            //}
            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        [HttpGet]
        public IActionResult CheckOut()
        {
            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        [HttpGet]
        public IActionResult CheckOutSuccess()
        {
            
            return View();
        }
        public IActionResult Partial_CheckOut()
        {
            return PartialView("Partial_CheckOut");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckOut(OrderViewModel req)
        {
            var code = new { success = false, code = -1 };
            if(ModelState.IsValid)
            {
                ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
                if (cart != null)
                {
                    Order order = new Order();
                    order.CustomerName = req.CustomerName;
                    order.Phone = req.Phone;
                    order.Address = req.Address;
                    cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Price = x.Price
                    }));
                    order.TotalAmount = cart.Items.Sum(x=>(x.Price * x.Quantity));
                    order.TypePayment = req.TypePayment;
                    order.CreatedDate = DateTime.Now;
                    order.CreatedBy = req.Phone;
                    order.ModifiedDate = DateTime.Now; 
                    Random rd = new Random();
                    order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    _db.Orders.Add(order);
                    _db.SaveChanges();
                    cart.ClearCart();
                    HttpContext.Session.Set("Cart", cart);
                    return RedirectToAction("CheckOutSuccess");
                }

            }
            return Json(code);
        }
        [HttpGet]
        public IActionResult Partial_Item_ThanhToan()
        {
            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null && cart.Items.Any())
            {
                return PartialView("Partial_Item_ThanhToan", cart.Items);
            }
            return PartialView("Partial_Item_ThanhToan");
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
        [HttpGet]
        public IActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null && cart.Items.Any())
            {
                return PartialView("Partial_Item_Cart", cart.Items);
            }
            return PartialView("Partial_Item_Cart");
        }
        [HttpPost]
        public IActionResult Update(int id, int quantity)
        {

            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                HttpContext.Session.Set("Cart", cart);
                return Json(new { success = true });
            }
            return Json(new { success = false });
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

        [HttpPost]
        public IActionResult DeleteAll()
        {

            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null)
            {
                cart.ClearCart();
                HttpContext.Session.Set("Cart", cart);
                return Json(new {success = true});
            }
            return Json(new { success = false });
        }
    }
}
