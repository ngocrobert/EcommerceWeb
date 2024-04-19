using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data;
using X.PagedList;
using WebBanHangOnline.Models.EF;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/products")]
    [Authorize(Roles = "Admin, Employee")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly string wwwrootDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads");
        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index(int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            //IEnumerable<Product> items = _db.Products.OrderByDescending(x => x.Id);
            IEnumerable<Product> items = _db.Products.Include(p => p.ProductCategory).OrderByDescending(x => x.Id);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile myFile)
        {
            if(myFile != null)
            {
                var path = Path.Combine(wwwrootDir, DateTime.Now.ToString() + Path.GetExtension(myFile.FileName));
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await myFile.CopyToAsync(stream);
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        [Route("add")]
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.ProductCategory = new SelectList(_db.ProductCategories.ToList(), "Id", "Title");
            return View();
        }
        [Route("add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add  (Product model, List<string> Images, List<int> rDefault)
        {
            if(ModelState.IsValid)
            {
                if(Images != null && Images.Count>0)
                {
                    for(int i=0; i<Images.Count; i++)
                    {
                        if(i+1 == rDefault[0])
                        {
                            model.Image = Images[i];
                            model.ProductImage.Add(new ProductImage
                            {
                                ProductId = model.Id,
                                Image = Images[i],
                                IsDefault = true
                            }) ;
                        }
                        else
                        {
                            model.ProductImage.Add(new ProductImage
                            {
                                ProductId = model.Id,
                                Image = Images[i],
                                IsDefault = false
                            });
                        }
                    }
                }

                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                if(string.IsNullOrEmpty(model.SeoTitle))
                {
                    model.SeoTitle = model.Title;
                }
                if(string.IsNullOrEmpty(model.Alias))
                {

                model.Alias = WebBanHangOnline.Models.Filter.FilterChar(model.Title);
                }
                _db.Products.Add(model);
                _db.SaveChanges();


                return RedirectToAction("Index");
            }
            ViewBag.ProductCategory = new SelectList(_db.ProductCategories.ToList(), "Id", "Title");
            return View(model);
        }
        [Route("imageDefault/{productId}")]
        [HttpGet]
        public IActionResult GetImageDefault(int productId)
        {
            var defaultImage = _db.ProductImages.FirstOrDefault(pi => pi.ProductId == productId && pi.IsDefault);
            if (defaultImage != null)
            {
                var defaultImagePath = defaultImage.Image;
                return Content(defaultImagePath);
            }
            return Content("");
        }
        [Route("edit")]
        [Route("edit/{id}")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.ProductCategory = new SelectList(_db.ProductCategories.ToList(), "Id", "Title");
            var item = _db.Products.Find(id);
            return View(item);
        }
        [Route("edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {

                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Filter.FilterChar(model.Title);
                _db.Products.Attach(model);
                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [Route("delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _db.Products.Find(id);
            if (item != null)
            {
                _db.Products.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }
        /// <summary>
        /// Chỉnh sửa trạng thái tin tức
        /// </summary>
        /// <param name="id">Id tin tức</param>
        /// <returns>1 đối tượng JSON</returns>
        [Route("IsActive")]
        [HttpPost]
        public IActionResult IsActive(int id)
        {
            var item = _db.Products.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                _db.Entry(item).State = EntityState.Modified;
                _db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }
            return Json(new { success = false });

        }
        [Route("IsHome")]
        [HttpPost]
        public IActionResult IsHome(int id)
        {
            var item = _db.Products.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                _db.Entry(item).State = EntityState.Modified;
                _db.SaveChanges();
                return Json(new { success = true, isHome = item.IsHome });
            }
            return Json(new { success = false });

        }
        [Route("IsSale")]
        [HttpPost]
        public IActionResult IsSale(int id)
        {
            var item = _db.Products.Find(id);
            if (item != null)
            {
                item.IsSale = !item.IsSale;
                _db.Entry(item).State = EntityState.Modified;
                _db.SaveChanges();
                return Json(new { success = true, isSale = item.IsSale });
            }
            return Json(new { success = false });

        }
    }
}
