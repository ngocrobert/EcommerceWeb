using Microsoft.AspNetCore.Mvc;

namespace WebBanHangOnline.ViewComponents
{
    public class Partial_SubcribeViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            
            return View("Partial_Subcribe");
        }
    }
}
