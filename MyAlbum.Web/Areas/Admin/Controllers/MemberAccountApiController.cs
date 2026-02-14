using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.EmployeeAccount;
using MyAlbum.Application.MemberAccount;
using MyAlbum.Models.Base;
using MyAlbum.Models.Category;
using MyAlbum.Models.EmployeeAccount;
using MyAlbum.Models.MemberAccount;
using MyAlbum.Models.UploadFiles;
using MyAlbum.Web.Models.MemberAccount;

namespace MyAlbum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class MemberAccountApiController : ControllerBase
    {
        private readonly IMemberAccountReadService _read;
        private readonly IMemberAccountCreateService _create;
        private readonly IMemberAccountUpdateService _update;
        public MemberAccountApiController(
            IMemberAccountReadService read,
            IMemberAccountCreateService create,
            IMemberAccountUpdateService update)
        {
            _read = read;
            _create = create;
            _update = update;
        }
        // /Admin/Api/MemberAccountApi?pageIndex=1&pageSize=10
        // /Admin/Api/MemberAccountApi?pageIndex=1&pageSize=10&Data.UserName=ma
        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<MemberAccountDto>>>> List([FromQuery] PageRequestBase<GetMemberAccountListReq> req, CancellationToken ct = default)
        {
            var result = await _read.GetMemberAccountListAsync(req, ct);
            return Ok(result);
        }

        [HttpGet("items")]
        public async Task<ActionResult<List<AlbumCategoryDto>>> GetItemListAsync([FromQuery] GetMemberAccountListReq req, CancellationToken ct = default)
        {
            var result = await _read.GetMemberAccountItemListAsync(req, ct);
            return Ok(result);
        }

        // GET /Admin/Api/MemberAccountApi/{memberId}/{accountId}
        [HttpGet("{memberId:guid}/{accountId:guid}")]
        public async Task<ActionResult<MemberAccountDto>> GetOne([FromRoute] Guid memberId, [FromRoute] Guid accountId, CancellationToken ct)
        {
            var req = new GetMemberAccountReq
            {
                AccountId = accountId,
                MemberId = memberId
            };

            var data = await _read.GetMemberAccountAsync(req, ct);
            if (data == null) return NotFound();
            return Ok(data);
        }

        // POST /Admin/Api/MemberAccountApi
        [HttpPost("CreateMemberWithUpload")]
        public async Task<ActionResult<Guid>> Create([FromForm] CreateMemberWithUploadForm form, CancellationToken ct)
        {
            CreateMemberReq req = new CreateMemberReq()
            {
                UserName = form.UserName,
                Password = form.Password,
                ConfirmPassword = form.ConfirmPassword,
                Email = form.Email,
                DisplayName = form.DisplayName,
            };

            var files = form.Files?.Where(f => f is not null && f.Length > 0).ToList() ?? new List<IFormFile>();
            var uploadFiles = new List<UploadFileStream>(files.Count);

            if (files.Count > 0)
            {
                const int maxFiles = 10;
                const long maxFileSize = 2 * 1024 * 1024;   // 單檔 2MB
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

                    var id = await _create.CreateMemberWithAccountAsync(req, uploadFiles, ct);
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
            else
            {
                var id = await _create.CreateMemberWithAccountAsync(req, uploadFiles, ct);
                return Ok(id);
            }
        }

        // PUT /Admin/Api/MemberAccountApi/{id}
        [HttpPut("{memberId:guid}/{accountId:guid}")]
        public async Task<ActionResult<bool>> Update([FromRoute] Guid memberId, [FromRoute] Guid accountId, [FromForm] UpdateMemberWithUploadForm form, CancellationToken ct)
        {
            UpdateMemberAccountReq req = new UpdateMemberAccountReq()
            {
                // 防止隨便改 body 的 id
                MemberId = memberId,
                AccountId = accountId,
                Password = form.Password,
                ConfirmPassword = form.ConfirmPassword,
                Email = form.Email,
                DisplayName = form.DisplayName,
                Status = form.Status
            };
            if (form.File is not null && form.File.Length > 0)
            {
                const long maxSize = 2 * 1024 * 1024; // 2MB
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

            var ok = await _update.UpdateMemberAccountAsync(req, ct);
            return Ok(ok);
        }

        // PATCH /Admin/Api/MemberAccountApi/{id}/status
        [HttpPatch("{memberId:guid}/{accountId:guid}/status")]
        public async Task<ActionResult<bool>> UpdateStatus([FromRoute] Guid memberId, [FromRoute] Guid accountId, [FromBody] UpdateStatusBody body, CancellationToken ct)
        {
            var req = new UpdateMemberAccountActiveReq
            {
                MemberId = memberId,
                AccountId = accountId,
                Status = body.Status
            };

            var ok = await _update.UpdateMemberAccountActiveAsync(req, ct);
            return Ok(ok);
        }
    }
}
