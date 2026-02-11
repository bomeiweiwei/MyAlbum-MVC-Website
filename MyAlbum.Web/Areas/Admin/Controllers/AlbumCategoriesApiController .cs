using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.Category;
using MyAlbum.Models.Base;
using MyAlbum.Models.Category;
using MyAlbum.Shared.Enums;

namespace MyAlbum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class AlbumCategoriesApiController : ControllerBase
    {
        private readonly IAlbumCategoryReadService _read;
        private readonly IAlbumCategoryCreateService _create;
        private readonly IAlbumCategoryUpdateService _update;
        public AlbumCategoriesApiController(
            IAlbumCategoryReadService read,
            IAlbumCategoryCreateService create,
            IAlbumCategoryUpdateService update)
        {
            _read = read;
            _create = create;
            _update = update;
        }
        // GET /Admin/Api/AlbumCategoriesApi?CategoryName=xxx
        [HttpGet]
        public async Task<ActionResult<List<AlbumCategoryDto>>> List([FromQuery] GetAlbumCategoryReq req, CancellationToken ct)
        {
            // List 不需要 AlbumCategoryId，避免被 ModelBinding 帶預設 Guid.Empty 造成困擾
            req.AlbumCategoryId = Guid.Empty;

            var data = await _read.GetAlbumCategoryListAsync(req, ct);
            return Ok(data);
        }

        // GET /Admin/Api/AlbumCategoriesApi/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AlbumCategoryDto>> GetOne([FromRoute] Guid id, CancellationToken ct)
        {
            var req = new GetAlbumCategoryReq
            {
                AlbumCategoryId = id
            };

            var data = await _read.GetAlbumCategoryAsync(req, ct);
            if (data == null) return NotFound();
            return Ok(data);
        }

        // POST /Admin/Api/AlbumCategoriesApi
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateAlbumCategoryReq req, CancellationToken ct)
        {
            var id = await _create.CreateAlbumCategoryAsync(req, ct);
            return Ok(id);
        }

        // PUT /Admin/Api/AlbumCategoriesApi/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<bool>> Update([FromRoute] Guid id, [FromBody] UpdateAlbumCategoryReq req, CancellationToken ct)
        {
            // 防止隨便改 body 的 id
            req.AlbumCategoryId = id;

            var ok = await _update.UpdateAlbumCategoryAsync(req, ct);
            return Ok(ok);
        }

        // PATCH /Admin/Api/AlbumCategoriesApi/{id}/status
        [HttpPatch("{id:guid}/status")]
        public async Task<ActionResult<bool>> UpdateStatus([FromRoute] Guid id, [FromBody] UpdateStatusBody body, CancellationToken ct)
        {
            var req = new UpdateAlbumCategoryActiveReq
            {
                AlbumCategoryId = id,
                Status = body.Status
            };

            var ok = await _update.UpdateAlbumCategoryActiveAsync(req, ct);
            return Ok(ok);
        }
    }
}
