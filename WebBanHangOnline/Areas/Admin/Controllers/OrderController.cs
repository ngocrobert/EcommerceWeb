using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Data;
using WebBanHangOnline.Models.EF;
using X.PagedList;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/order")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(int? page)
        {
            IEnumerable<Order> items = _db.Orders.OrderByDescending(x => x.CreatedDate).ToList();

            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        [Route("ViewDetail/{id}")]
        public IActionResult ViewDetail(int id)
        {
            //var item = _db.Orders.Find(id);
            //item.OrderDetails = _db.OrderDetails.Where(x => x.OrderId == id).ToList();
            var item = _db.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Product).FirstOrDefault(o => o.Id == id);
            return View(item);
        }
        //[Route("partial_sanpham")]
        //public IActionResult Partial_SanPham(int id)
        //{
        //    var items = _db.OrderDetails.Where(x => x.OrderId == id).ToList();

        //    //foreach(var item in items)
        //    //{
        //    //    var product = _db.Products.FirstOrDefault(x => x.Id == item.ProductId);
        //    //    if (product != null)
        //    //    {
        //    //        item.Product = product;
        //    //    }
        //    //}

        //    return PartialView(items);
        //}
        [Route("UpdateTT")]
        [HttpPost]
        public IActionResult UpdateTT(int id, int trangthai)
        {
            var item = _db.Orders.Find(id);
            if(item != null)
            {
                _db.Orders.Attach(item);
                item.TypePayment = trangthai;
                _db.Entry(item).Property(x=>x.TypePayment).IsModified = true;
                _db.SaveChanges();
                return Json(new { mess = "Success", success = true });
            }
            return Json(new { mess = "Fail", success = false });
        }

        
    }
}
