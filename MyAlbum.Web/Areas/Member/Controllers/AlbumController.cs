using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.Album;
using MyAlbum.Application.Category;
using MyAlbum.Infrastructure.EF.Models;
using MyAlbum.Models.Album;
using MyAlbum.Models.Base;
using MyAlbum.Models.Category;
using MyAlbum.Models.UploadFiles;
using MyAlbum.Web.Models.Album;

namespace MyAlbum.Web.Areas.Member.Controllers
{
    [Area("Member")]
    public class AlbumController : Controller
    {
        private readonly IMemberAlbumReadService _read;
        private readonly IAlbumUpdateService _update;
        private readonly IAlbumCreateService _create;
        private readonly IAlbumCategoryReadService _categoryRead;

        public AlbumController(
            IMemberAlbumReadService read,
             IAlbumUpdateService update,
             IAlbumCreateService create,
             IAlbumCategoryReadService categoryRead)
        {
            _read = read;
            _update = update;
            _create = create;
            _categoryRead = categoryRead;
        }

        public async Task<IActionResult> Index()
        {
            GetAlbumListReq req = new GetAlbumListReq();
            var dataList = await _read.GetAlbumListAsync(req);
            return View(dataList);
        }

        public async Task<IActionResult> Create()
        {
            var req = new GetAlbumCategoryListReq
            {
                Status = Shared.Enums.Status.Active
            };

            var categories = await _categoryRead.GetAlbumCategoryItemListAsync(req);

            var vm = new CreateAlbumViewModel
            {
                CategoryList = categories
            };

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAlbumViewModel model, CancellationToken ct)
        {
            // 1) 基本驗證
            if (model.File is null || model.File.Length == 0)
                ModelState.AddModelError(nameof(model.File), "請選擇封面圖片");

            if (!ModelState.IsValid)
            {
                await ReloadCategories(model);
                return View(model);
            }

            // 2) 取登入者 AccountId
            var accountIdValue = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(accountIdValue, out var accountId))
                return Unauthorized();

            // 3) 檔案規則
            const long maxSize = 10 * 1024 * 1024; // 10MB
            if (model.File.Length > maxSize)
            {
                ModelState.AddModelError(nameof(model.File), "檔案過大，限制 10MB");
                await ReloadCategories(model);
                return View(model);
            }

            var allowedExt = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(model.File.FileName).ToLowerInvariant();
            if (!allowedExt.Contains(ext))
            {
                ModelState.AddModelError(nameof(model.File), "僅允許 jpg / jpeg / png");
                await ReloadCategories(model);
                return View(model);
            }

            var allowedContentTypes = new[] { "image/jpeg", "image/png" };
            if (!allowedContentTypes.Contains(model.File.ContentType))
            {
                ModelState.AddModelError(nameof(model.File), "檔案格式不正確");
                await ReloadCategories(model);
                return View(model);
            }

            // 4) 組 req
            var createAlbumReq = new CreateAlbumReq
            {
                AlbumCategoryId = model.AlbumCategoryId,
                OwnerAccountId = accountId,
                Title = model.Title,
                Description = model.Description
            };

            // 5) Stream 生命週期：Controller 這層保證釋放
            await using var stream = model.File.OpenReadStream();

            IReadOnlyList<UploadFileStream> files = new List<UploadFileStream>
            {
                new UploadFileStream
                {
                    FileName = model.File.FileName,
                    ContentType = model.File.ContentType,
                    Length = model.File.Length,
                    Stream = stream
                }
            };

            await _create.CreateAlbumAsync(createAlbumReq, files, ct);

            return RedirectToAction(nameof(Index));
        }


        private async Task ReloadCategories(CreateAlbumViewModel model)
        {
            var req = new GetAlbumCategoryListReq
            {
                Status = Shared.Enums.Status.Active
            };

            model.CategoryList =
                await _categoryRead.GetAlbumCategoryItemListAsync(req);
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
