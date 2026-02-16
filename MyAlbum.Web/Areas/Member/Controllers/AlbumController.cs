using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.Album;
using MyAlbum.Models.Album;

namespace MyAlbum.Web.Areas.Member.Controllers
{
    [Area("Member")]
    public class AlbumController : Controller
    {
        private readonly IMemberAlbumReadService _read;
        public AlbumController(IMemberAlbumReadService read)
        {
            _read = read;
        }

        public async Task<IActionResult> Index()
        {
            GetAlbumListReq req= new GetAlbumListReq();
            var dataList = await _read.GetAlbumListAsync(req);
            return View(dataList);
        }
    }
}
