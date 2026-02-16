using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.AlbumPhoto;
using MyAlbum.Models.Album;
using MyAlbum.Models.AlbumPhoto;

namespace MyAlbum.Web.Areas.Member.Controllers
{
    [Area("Member")]
    public class AlbumPhotoController : Controller
    {
        private readonly IMemberAlbumPhotoReadService _read;
        public AlbumPhotoController(
            IMemberAlbumPhotoReadService read)
        {
            _read = read;
        }

        public async Task<IActionResult> Index(Guid albumId)
        {
            GetAlbumPhotoReq req = new GetAlbumPhotoReq();
            req.AlbumId = albumId;
            var dataList = await _read.GetAlbumPhotoListAsync(req);
            return View(dataList);
        }
    }
}
