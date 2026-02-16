using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.Album;
using MyAlbum.Infrastructure.EF.Models;
using MyAlbum.Models.Album;
using MyAlbum.Models.Base;

namespace MyAlbum.Web.Areas.Member.Controllers
{
    [Area("Member")]
    public class AlbumController : Controller
    {
        private readonly IMemberAlbumReadService _read;
        private readonly IAlbumUpdateService _update;
        public AlbumController(
            IMemberAlbumReadService read,
             IAlbumUpdateService update)
        {
            _read = read;
            _update = update;
        }

        public async Task<IActionResult> Index()
        {
            GetAlbumListReq req = new GetAlbumListReq();
            var dataList = await _read.GetAlbumListAsync(req);
            return View(dataList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Active(UpdateMemberAlbumActiveReq model)
        {
            var req = new UpdateAlbumActiveReq
            {
                AlbumId = model.AlbumId,
                Status = Shared.Enums.Status.Active
            };

            var ok = await _update.UpdateAlbumActiveAsync(req);
            if (!ok)
            {
                TempData["Error"] = "操作失敗";
            }
            else
            {
                TempData["Success"] = "已停用成功";
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disable(UpdateMemberAlbumActiveReq model)
        {
            var req = new UpdateAlbumActiveReq
            {
                AlbumId = model.AlbumId,
                Status = Shared.Enums.Status.Disabled
            };

            var ok = await _update.UpdateAlbumActiveAsync(req);
            if (!ok)
            {
                TempData["Error"] = "操作失敗";
            }
            else
            {
                TempData["Success"] = "已停用成功";
            }

            return RedirectToAction("Index");
        }
    }
}
