using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.AlbumComment;
using MyAlbum.Application.AlbumPhoto;
using MyAlbum.Models.AlbumComment;
using MyAlbum.Models.AlbumPhoto;

namespace MyAlbum.Web.Areas.Member.Controllers
{
    [Area("Member")]
    public class AlbumCommentController : Controller
    {
        private readonly IMemberAlbumCommentReadService _read;
        private readonly IMemberAlbumPhotoReadService _photoRead;
        public AlbumCommentController(
            IMemberAlbumCommentReadService read,
            IMemberAlbumPhotoReadService photoRead)
        {
            _read = read;
            _photoRead = photoRead;
        }

        public async Task<IActionResult> Index(Guid albumPhotoId)
        {
            var photoReq = new GetAlbumPhotoReq()
            {
                AlbumPhotoId = albumPhotoId
            };
            var data = await _photoRead.GetAlbumPhotoAsync(photoReq);
            if (data == null)
                return NotFound();

            ViewBag.AlbumPhoto = data;

            var req = new GetAlbumCommentListReq()
            {
                AlbumPhotoId = albumPhotoId
            };
            var comments = await _read.GetAlbumCommentListAsync(req);

            return View(comments);
        }
    }
}
