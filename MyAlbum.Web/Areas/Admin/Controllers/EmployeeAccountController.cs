using Microsoft.AspNetCore.Mvc;

namespace MyAlbum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeAccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
