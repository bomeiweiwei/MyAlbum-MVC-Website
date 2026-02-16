using Microsoft.AspNetCore.Mvc;

namespace MyAlbum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AlbumPhotoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
