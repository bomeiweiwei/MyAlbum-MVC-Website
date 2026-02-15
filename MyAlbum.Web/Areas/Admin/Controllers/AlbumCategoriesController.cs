using Microsoft.AspNetCore.Mvc;

namespace MyAlbum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AlbumCategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
