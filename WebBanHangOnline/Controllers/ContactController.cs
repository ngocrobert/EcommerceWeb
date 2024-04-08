using Microsoft.AspNetCore.Mvc;

namespace WebBanHangOnline.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
