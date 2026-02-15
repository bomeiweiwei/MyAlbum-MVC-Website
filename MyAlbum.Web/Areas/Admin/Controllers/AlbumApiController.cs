using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.Album;
using MyAlbum.Application.MemberAccount;
using MyAlbum.Infrastructure.EF.Models;
using MyAlbum.Models.Album;
using MyAlbum.Models.Base;
using MyAlbum.Models.Category;
using MyAlbum.Models.MemberAccount;
using MyAlbum.Models.UploadFiles;
using MyAlbum.Web.Models.Album;
using MyAlbum.Web.Models.MemberAccount;

namespace MyAlbum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class AlbumApiController : ControllerBase
    {
        private readonly IAlbumReadService _read;
        private readonly IAlbumCreateService _create;
        private readonly IAlbumUpdateService _update;
        public AlbumApiController(
            IAlbumReadService read,
            IAlbumCreateService create,
            IAlbumUpdateService update)
        {
            _read = read;
            _create = create;
            _update = update;
        }
        // /Admin/Api/AlbumApi?pageIndex=1&pageSize=10
        // /Admin/Api/AlbumApi?pageIndex=1&pageSize=10&Data.Title=Te&Data.Status=1
        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<AlbumDto>>>> List([FromQuery] PageRequestBase<GetAlbumListReq> req, CancellationToken ct = default)
        {
            var result = await _read.GetAlbumListAsync(req, ct);
            return Ok(result);
        }

        [HttpGet("items")]
        public async Task<ActionResult<List<AlbumCategoryDto>>> GetItemListAsync([FromQuery] GetAlbumListReq req, CancellationToken ct = default)
        {
            var result = await _read.GetAlbumListItemAsync(req, ct);
            return Ok(result);
        }

        [HttpGet("{albumId:guid}")]
        public async Task<ActionResult<AlbumDto>> GetOne([FromRoute] Guid albumId, CancellationToken ct)
        {
            var req = new GetAlbumReq
            {
                AlbumId = albumId
            };

            var data = await _read.GetAlbumAsync(req, ct);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPost("CreateAlbumWithUpload")]
        public async Task<ActionResult<Guid>> Create([FromForm] CreateAlbumWithUploadForm form, CancellationToken ct)
        {
            CreateAlbumReq req = new CreateAlbumReq()
            {
                AlbumCategoryId = form.AlbumCategoryId,
                OwnerAccountId = form.OwnerAccountId,
                Title = form.Title,
                Description = form.Description
            };

            var files = form.Files?.Where(f => f is not null && f.Length > 0).ToList() ?? new List<IFormFile>();
            var uploadFiles = new List<UploadFileStream>(files.Count);

            if (files.Count == 0)
                return BadRequest("至少要上傳 1 張圖片");

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

                var id = await _create.CreateAlbumAsync(req, uploadFiles, ct);
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

        [HttpPut("{albumId:guid}")]
        public async Task<ActionResult<bool>> Update([FromRoute] Guid albumId, [FromForm] UpdateAlbumWithUploadForm form, CancellationToken ct)
        {
            UpdateAlbumReq req = new UpdateAlbumReq()
            {
                // 防止隨便改 body 的 id
                AlbumId = albumId,
                AlbumCategoryId = form.AlbumCategoryId,
                OwnerAccountId = form.OwnerAccountId,
                Title = form.Title,
                Description = form.Description,
                Status = form.Status,
            };

            var files = form.Files?.Where(f => f is not null && f.Length > 0).ToList() ?? new List<IFormFile>();
            var uploadFiles = new List<UploadFileStream>(files.Count);

            if (files.Count > 0)
            {
                const int maxFiles = 10;
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

                    var ok = await _update.UpdateAlbumAsync(req, uploadFiles, ct);
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
                var ok = await _update.UpdateAlbumAsync(req, uploadFiles, ct);
                return Ok(ok);
            }
        }

        [HttpPatch("{albumId:guid}/status")]
        public async Task<ActionResult<bool>> UpdateStatus([FromRoute] Guid albumId, [FromBody] UpdateStatusBody body, CancellationToken ct)
        {
            var req = new UpdateAlbumActiveReq
            {
                AlbumId = albumId,
                Status = body.Status
            };

            var ok = await _update.UpdateAlbumActiveAsync(req, ct);
            return Ok(ok);
        }
    }
}
