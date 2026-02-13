using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.Album;
using MyAlbum.Application.MemberAccount;
using MyAlbum.Infrastructure.EF.Models;
using MyAlbum.Models.Album;
using MyAlbum.Models.Base;
using MyAlbum.Models.MemberAccount;
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

            if (form.File is not null && form.File.Length > 0)
            {
                const long maxSize = 10 * 1024 * 1024; // 10MB
                if (form.File.Length > maxSize)
                    return BadRequest("檔案過大，限制 10MB");

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var ext = Path.GetExtension(form.File.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                    return BadRequest("僅允許 jpg / png 格式");

                var allowedContentTypes = new[] { "image/jpeg", "image/png" };
                if (!allowedContentTypes.Contains(form.File.ContentType))
                    return BadRequest("檔案格式不正確");

                await using var ms = new MemoryStream();
                await form.File.CopyToAsync(ms, ct);

                req.FileBytes = ms.ToArray();
                req.FileName = form.File.FileName;
                //req.ContentType = form.File.ContentType;
            }

            var id = await _create.CreateAlbumAsync(req, ct);
            return Ok(id);
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
            if (form.File is not null && form.File.Length > 0)
            {
                const long maxSize = 10 * 1024 * 1024; // 10MB
                if (form.File.Length > maxSize)
                    return BadRequest("檔案過大，限制 2MB");

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var ext = Path.GetExtension(form.File.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                    return BadRequest("僅允許 jpg / png 格式");

                var allowedContentTypes = new[] { "image/jpeg", "image/png" };
                if (!allowedContentTypes.Contains(form.File.ContentType))
                    return BadRequest("檔案格式不正確");

                await using var ms = new MemoryStream();
                await form.File.CopyToAsync(ms, ct);

                req.FileBytes = ms.ToArray();
                req.FileName = form.File.FileName;
                //req.ContentType = form.File.ContentType;
            }

            var ok = await _update.UpdateAlbumAsync(req, ct);
            return Ok(ok);
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
