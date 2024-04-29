using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using WebBanHangOnline.Data;
using WebBanHangOnline.Extensions;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebBanHangOnline.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private IHostingEnvironment Environment { get; set; }
        public IConfiguration Configuration { get; set; }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ShoppingCartController(ApplicationDbContext db, IConfiguration _configuration, IHostingEnvironment environment, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            Configuration = _configuration;
            Environment = environment;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //[AllowAnonymous]
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
        [AllowAnonymous]
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
        [AllowAnonymous]

        public IActionResult CheckOutSuccess()
        {
            
            return View();
        }
        [AllowAnonymous]

        public IActionResult Partial_CheckOut()
        {
            //var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            //if (user != null)
            //{
            //    ViewBag.User = user;
            //}
            return PartialView("Partial_CheckOut");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]

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
                    order.Email = req.Email;
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

                    // send mail cho khach hang
                    var strSanPham = "";
                    var thanhtien = decimal.Zero;
                    var TongTien = decimal.Zero;
                    foreach(var sp in cart.Items)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>"+sp.ProductName+"</td>";
                        strSanPham += "<td>" + sp.Quantity + "</td>";
                        strSanPham += "<td>" + WebBanHangOnline.Common.Common.FormatNumber(sp.TotalPrice) + "</td>";
                        strSanPham += "</tr>";
                        thanhtien += sp.Price * sp.Quantity;
                    }
                    TongTien = thanhtien;
                    string body = this.ContentBody(order.Code, strSanPham,  order.CustomerName, order.Phone,order.Address,  req.Email, thanhtien, TongTien, "templates//send2.html");
                    this.SendHtmlFormattedEmail(req.Email, "Đơn hàng #"+order.Code, body.ToString());
                    //send email admin
                    string bodyAdmin = this.ContentBody(order.Code, strSanPham, order.CustomerName, order.Phone, order.Address, req.Email, thanhtien, TongTien, "templates//send1.html");
                    string emailAdmin = this.Configuration.GetValue<string>("Smtp:EmailAdmin");
                    this.SendHtmlFormattedEmail(emailAdmin, "Đơn hàng #" + order.Code, bodyAdmin.ToString());

                    //xóa số lượng SP đã được đặt
                    foreach(var sp in cart.Items)
                    {
                        var product = _db.Products.FirstOrDefault(x => x.Id == sp.ProductId);
                        if(product!=null)
                        {
                            product.Quantity -= sp.Quantity;
                            _db.Products.Update(product);
                            _db.SaveChanges();
                        }
                        else
                        {
                            return Json(code);
                        }
                    }

                    cart.ClearCart();
                    HttpContext.Session.Set("Cart", cart);
                    return RedirectToAction("CheckOutSuccess");
                }

            }
            return Json(code);
        }

        private string ContentBody(string maDon, string sanPham, string tenKhach, string phone, string diachi, string email, decimal thanhtien, decimal tongtien, string template)
        {
            string body = string.Empty;
            string path = Path.Combine(this.Environment.WebRootPath, template);
            using (StreamReader reader = new StreamReader(path))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{{MaDon}}", maDon);
            body = body.Replace("{{SanPham}}", sanPham);
            body = body.Replace("{{TenKhachHang}}", tenKhach);
            body = body.Replace("{{Phone}}", phone);
            body = body.Replace("{{DiaChiNhan}}", diachi);
            body = body.Replace("{{Email}}", email);
            body = body.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
            body = body.Replace("{{ThanhTien}}", WebBanHangOnline.Common.Common.FormatNumber(thanhtien,0));

            body = body.Replace("{{TongTien}}", WebBanHangOnline.Common.Common.FormatNumber(tongtien, 0));
            return body;

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

        [HttpGet]
        [AllowAnonymous]

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
