using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.AlbumPhoto;
using MyAlbum.Models.Album;
using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Models.UploadFiles;
using MyAlbum.Web.Models.AlbumPhoto;

namespace MyAlbum.Web.Areas.Member.Controllers
{
    [Area("Member")]
    public class AlbumPhotoController : Controller
    {
        private readonly IMemberAlbumPhotoReadService _read;
        private readonly IAlbumPhotoUpdateService _update;
        private readonly IAlbumPhotoCreateService _create;
        public AlbumPhotoController(
            IMemberAlbumPhotoReadService read,
            IAlbumPhotoUpdateService update,
            IAlbumPhotoCreateService create)
        {
            _read = read;
            _update = update;
            _create = create;
        }

        public async Task<IActionResult> Index(Guid albumId)
        {
            ViewBag.AlbumId = albumId;

            GetAlbumPhotoReq req = new GetAlbumPhotoReq();
            req.AlbumId = albumId;
            var dataList = await _read.GetAlbumPhotoListAsync(req);
            return View(dataList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePhotos([FromForm] CreateAlbumPhotosViewModel model, CancellationToken ct)
        {
            // 1) 基本防呆
            if (model.AlbumId == Guid.Empty)
                ModelState.AddModelError(nameof(model.AlbumId), "Id 不可為空");

            if (model.Files is null || model.Files.Count == 0)
                ModelState.AddModelError(nameof(model.Files), "請選擇至少 1 張圖片");

            // 2) 逐檔檢核
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var allowedContentTypes = new[] { "image/jpeg", "image/png" };
            const long maxSize = 10 * 1024 * 1024; // 10MB

            if (model.Files is not null)
            {
                for (int i = 0; i < model.Files.Count; i++)
                {
                    var file = model.Files[i];

                    if (file == null || file.Length == 0)
                    {
                        ModelState.AddModelError($"Files[{i}]", "檔案不可為空");
                        continue;
                    }

                    var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
                    if (string.IsNullOrWhiteSpace(ext) || !allowedExtensions.Contains(ext))
                    {
                        ModelState.AddModelError($"Files[{i}]", $"不允許的副檔名：{ext}，僅允許 jpg/jpeg/png");
                        continue;
                    }

                    if (!allowedContentTypes.Contains(file.ContentType))
                    {
                        ModelState.AddModelError($"Files[{i}]", "檔案格式不正確");
                        continue;
                    }

                    if (file.Length > maxSize)
                    {
                        ModelState.AddModelError($"Files[{i}]", "檔案大小超過 10MB");
                        continue;
                    }
                }
            }

            // 3) 有錯直接回 400 + errors（給前端顯示）
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "驗證失敗",
                    errors = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
                    )
                });
            }

            // 4) 取登入者
            var accountIdValue = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(accountIdValue, out var accountId))
                return Unauthorized();

            // 5) 組成 uploadFiles
            var files = model.Files!.Where(f => f is not null && f.Length > 0).ToList();
            var uploadFiles = new List<UploadFileStream>(files.Count);

            try
            {
                var req = new CreateAlbumPhotoReq { AlbumId = model.AlbumId };

                foreach (var file in files)
                {
                    uploadFiles.Add(new UploadFileStream
                    {
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        Length = file.Length,
                        Stream = file.OpenReadStream()
                    });
                }

                await _create.CreateAlbumPhotoAsync(req, uploadFiles, ct);

                return Ok(new { ok = true, albumId = model.AlbumId });
            }
            finally
            {
                foreach (var f in uploadFiles)
                {
                    try { await f.Stream.DisposeAsync(); } catch { }
                }
            }
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
