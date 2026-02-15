using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.Album;
using MyAlbum.Application.AlbumComment;
using MyAlbum.Models.AlbumComment;
using MyAlbum.Models.Base;
using MyAlbum.Models.Category;

namespace MyAlbum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class AlbumCommentApiController : ControllerBase
    {
        private readonly IAlbumCommentReadService _read;
        private readonly IAlbumCommentCreateService _create;
        private readonly IAlbumCommentUpdateService _update;
        public AlbumCommentApiController(
            IAlbumCommentReadService read,
            IAlbumCommentCreateService create,
            IAlbumCommentUpdateService update)
        {
            _read = read;
            _create = create;
            _update = update;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<AlbumCommentDto>>>> List([FromQuery] PageRequestBase<GetAlbumCommentListReq> req, CancellationToken ct = default)
        {
            var result = await _read.GetAlbumCommentListAsync(req, ct);
            return Ok(result);
        }

        // GET /Admin/Api/AlbumCategoriesApi/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AlbumCommentDto>> GetOne([FromRoute] Guid id, CancellationToken ct)
        {
            var req = new GetAlbumCommentReq
            {
                AlbumCommentId = id
            };

            var data = await _read.GetAlbumCommentAsync(req, ct);
            if (data == null) return NotFound();
            return Ok(data);
        }

        // POST /Admin/Api/AlbumCategoriesApi
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateAlbumCommentReq req, CancellationToken ct)
        {
            var id = await _create.CreateAlbumCommentAsync(req, ct);
            return Ok(id);
        }

        // PUT /Admin/Api/AlbumCategoriesApi/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<bool>> Update([FromRoute] Guid id, [FromBody] UpdateAlbumCommentReq req, CancellationToken ct)
        {
            // 防止隨便改 body 的 id
            req.AlbumCommentId = id;

            var ok = await _update.UpdateAlbumCommentAsync(req, ct);
            return Ok(ok);
        }

        // PATCH /Admin/Api/AlbumCategoriesApi/{id}/status
        [HttpPatch("{id:guid}/status")]
        public async Task<ActionResult<bool>> UpdateStatus([FromRoute] Guid id, [FromBody] UpdateStatusBody body, CancellationToken ct)
        {
            var req = new UpdateAlbumCommentActiveReq
            {
                AlbumCommentId = id,
                Status = body.Status
            };

            var ok = await _update.UpdateAlbumCommentActiveAsync(req, ct);
            return Ok(ok);
        }

    }
}
