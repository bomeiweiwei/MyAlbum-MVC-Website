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
        private readonly IAlbumPhotoUpdateService _update;
        public AlbumPhotoController(
            IMemberAlbumPhotoReadService read,
            IAlbumPhotoUpdateService update)
        {
            _read = read;
            _update = update;
        }

        public async Task<IActionResult> Index(Guid albumId)
        {
            GetAlbumPhotoReq req = new GetAlbumPhotoReq();
            req.AlbumId = albumId;
            var dataList = await _read.GetAlbumPhotoListAsync(req);
            return View(dataList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Active(UpdateMemberAlbumPhotoActiveReq model)
        {
            var req = new UpdateAlbumPhotoActiveReq
            {
                AlbumPhotoId = model.AlbumPhotoId,
                Status = Shared.Enums.Status.Active
            };

            var ok = await _update.UpdateAlbumPhotoActiveAsync(req);
            if (!ok)
            {
                TempData["Error"] = "操作失敗";
            }
            else
            {
                TempData["Success"] = "已停用成功";
            }

            return RedirectToAction("Index", new { albumId = model.AlbumId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disable(UpdateMemberAlbumPhotoActiveReq model)
        {
            var req = new UpdateAlbumPhotoActiveReq
            {
                AlbumPhotoId = model.AlbumPhotoId,
                Status = Shared.Enums.Status.Disabled
            };

            var ok = await _update.UpdateAlbumPhotoActiveAsync(req);
            if (!ok)
            {
                TempData["Error"] = "操作失敗";
            }
            else
            {
                TempData["Success"] = "已停用成功";
            }

            return RedirectToAction("Index", new { albumId = model.AlbumId });
        }
    }
}
