using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.Album;
using MyAlbum.Application.AlbumPhoto;
using MyAlbum.Models.Album;
using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Models.Base;
using MyAlbum.Models.Category;
using MyAlbum.Models.UploadFiles;
using MyAlbum.Web.Models.AlbumPhoto;

namespace MyAlbum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class AlbumPhotoApiController : ControllerBase
    {
        private readonly IAlbumPhotoReadService _read;
        private readonly IAlbumPhotoCreateService _create;
        private readonly IAlbumPhotoUpdateService _update;
        public AlbumPhotoApiController(
            IAlbumPhotoReadService read,
            IAlbumPhotoCreateService create,
            IAlbumPhotoUpdateService update)
        {
            _read = read;
            _create = create;
            _update = update;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<AlbumPhotoDto>>>> List([FromQuery] PageRequestBase<GetAlbumPhotoReq> req, CancellationToken ct = default)
        {
            var result = await _read.GetAlbumPhotoListAsync(req, ct);
            return Ok(result);
        }

        // GET /Admin/Api/AlbumCategoriesApi/{id}
        [HttpGet("{albumPhotoId:guid}/{albumId:guid}")]
        public async Task<ActionResult<AlbumPhotoDto>> GetOne([FromRoute] Guid albumPhotoId, [FromRoute] Guid albumId, CancellationToken ct)
        {
            var req = new GetAlbumPhotoReq
            {
                AlbumPhotoId = albumPhotoId,
                AlbumId = albumId,
            };

            var data = await _read.GetAlbumPhotoAsync(req, ct);
            if (data == null) return NotFound();
            return Ok(data);
        }

        // POST /Admin/Api/AlbumCategoriesApi
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromForm] CreateAlbumPhotoWithUploadForm form, CancellationToken ct)
        {
            CreateAlbumPhotoReq req = new CreateAlbumPhotoReq()
            {
                AlbumId = form.AlbumId,
            };

            var files = form.Files?.Where(f => f is not null && f.Length > 0).ToList() ?? new List<IFormFile>();
            var uploadFiles = new List<UploadFileStream>(files.Count);

            if (files.Count == 0)
                return BadRequest("至少要上傳 1 張圖片");

            const int maxFiles = 5;
            const long maxFileSize = 10 * 1024 * 1024;   // 單檔 10MB
            const long maxTotalSize = 50 * 1024 * 1024; // 總量 50MB
            var allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    ".jpg", ".jpeg", ".png"
                };

            var allowedContentTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "image/jpeg", "image/png"
                };

            if (files.Count > maxFiles)
                return BadRequest($"最多允許上傳 {maxFiles} 個檔案");

            long totalSize = 0;
            try
            {
                foreach (var file in files)
                {
                    totalSize += file.Length;
                    if (totalSize > maxTotalSize)
                        return BadRequest($"總上傳大小不可超過 {maxTotalSize / (1024 * 1024)}MB");

                    if (file.Length > maxFileSize)
                        return BadRequest($"單檔不可超過 {maxFileSize / (1024 * 1024)}MB");

                    var ext = Path.GetExtension(file.FileName);
                    if (string.IsNullOrWhiteSpace(ext) || !allowedExtensions.Contains(ext))
                        return BadRequest($"不允許的副檔名：{ext}，僅允許 jpg/jpeg/png");

                    if (!allowedContentTypes.Contains(file.ContentType))
                        return BadRequest($"不允許的 ContentType：{file.ContentType}");

                    var stream = file.OpenReadStream();

                    uploadFiles.Add(new UploadFileStream
                    {
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        Length = file.Length,
                        Stream = stream
                    });
                }

                var id = await _create.CreateAlbumPhotoAsync(req, uploadFiles, ct);
                return Ok(id);
            }
            finally
            {
                foreach (var f in uploadFiles)
                {
                    try { await f.Stream.DisposeAsync(); } catch { }
                }
            }
        }

        // PUT /Admin/Api/AlbumCategoriesApi/{id}
        [HttpPut("{albumPhotoId:guid}/{albumId:guid}")]
        public async Task<ActionResult<bool>> Update([FromRoute] Guid albumPhotoId, [FromRoute] Guid albumId, [FromForm] UpdateAlbumPhotoWithUploadForm form, CancellationToken ct)
        {
            UpdateAlbumPhotoReq req = new UpdateAlbumPhotoReq();
            // 防止隨便改 body 的 id
            req.AlbumPhotoId = albumPhotoId;
            req.AlbumId = albumId;
            req.SortOrder = form.SortOrder;
            req.Status = form.Status;

            var files = form.Files?.Where(f => f is not null && f.Length > 0).ToList() ?? new List<IFormFile>();
            var uploadFiles = new List<UploadFileStream>(files.Count);

            if (files.Count > 0)
            {
                const int maxFiles = 1;
                const long maxFileSize = 10 * 1024 * 1024;   // 單檔 10MB
                const long maxTotalSize = 10 * 1024 * 1024; // 總量 10MB
                var allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    ".jpg", ".jpeg", ".png"
                };

                var allowedContentTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "image/jpeg", "image/png"
                };

                if (files.Count > maxFiles)
                    return BadRequest($"最多允許上傳 {maxFiles} 個檔案");

                long totalSize = 0;
                try
                {
                    foreach (var file in files)
                    {
                        totalSize += file.Length;
                        if (totalSize > maxTotalSize)
                            return BadRequest($"總上傳大小不可超過 {maxTotalSize / (1024 * 1024)}MB");

                        if (file.Length > maxFileSize)
                            return BadRequest($"單檔不可超過 {maxFileSize / (1024 * 1024)}MB");

                        var ext = Path.GetExtension(file.FileName);
                        if (string.IsNullOrWhiteSpace(ext) || !allowedExtensions.Contains(ext))
                            return BadRequest($"不允許的副檔名：{ext}，僅允許 jpg/jpeg/png");

                        if (!allowedContentTypes.Contains(file.ContentType))
                            return BadRequest($"不允許的 ContentType：{file.ContentType}");

                        var stream = file.OpenReadStream();

                        uploadFiles.Add(new UploadFileStream
                        {
                            FileName = file.FileName,
                            ContentType = file.ContentType,
                            Length = file.Length,
                            Stream = stream
                        });
                    }

                    var ok = await _update.UpdateAlbumPhotoAsync(req, uploadFiles, ct);
                    return Ok(ok);
                }
                finally
                {
                    foreach (var f in uploadFiles)
                    {
                        try { await f.Stream.DisposeAsync(); } catch { }
                    }
                }
            }
            else
            {
                var ok = await _update.UpdateAlbumPhotoAsync(req, uploadFiles, ct);
                return Ok(ok);
            }
        }

        // PATCH /Admin/Api/AlbumCategoriesApi/{id}/status
        [HttpPatch("{id:guid}/status")]
        public async Task<ActionResult<bool>> UpdateStatus([FromRoute] Guid id, [FromBody] UpdateStatusBody body, CancellationToken ct)
        {
            var req = new UpdateAlbumPhotoActiveReq
            {
                AlbumPhotoId = id,
                Status = body.Status
            };

            var ok = await _update.UpdateAlbumPhotoActiveAsync(req, ct);
            return Ok(ok);
        }
    }
}
