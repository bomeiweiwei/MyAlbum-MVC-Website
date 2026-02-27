using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.Category;
using MyAlbum.Application.EmployeeAccount;
using MyAlbum.Models.Base;
using MyAlbum.Models.Category;
using MyAlbum.Models.EmployeeAccount;
using MyAlbum.Shared.Enums;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace MyAlbum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class EmployeeAccountApiController : ControllerBase
    {
        private readonly IEmployeeAccountReadService _read;
        private readonly IEmployeeAccountCreateService _create;
        private readonly IEmployeeAccountUpdateService _update;
        public EmployeeAccountApiController(
            IEmployeeAccountReadService read,
            IEmployeeAccountCreateService create,
            IEmployeeAccountUpdateService update)
        {
            _read = read;
            _create = create;
            _update = update;
        }
        // https://localhost:44343/Admin/Api/EmployeeAccountApi?pageIndex=1&pageSize=10
        // https://localhost:44343/Admin/Api/EmployeeAccountApi?pageIndex=1&pageSize=10&Data.UserName=jer
        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<EmployeeAccountDto>>>> List([FromQuery] PageRequestBase<GetEmployeeAccountListReq> req, CancellationToken ct = default)
        {
            var result = await _read.GetEmployeeAccountListAsync(req, ct);
            return Ok(result);
        }

        // GET /Admin/Api/EmployeeAccountApi/{employeeId}/{accountId}
        [HttpGet("{employeeId:guid}/{accountId:guid}")]
        public async Task<ActionResult<EmployeeAccountDto>> GetOne([FromRoute] Guid employeeId, [FromRoute] Guid accountId, CancellationToken ct)
        {
            var req = new GetEmployeeAccountReq
            {
                AccountId = accountId,
                EmployeeId = employeeId
            };

            var data = await _read.GetEmployeeAccountAsync(req, ct);
            if (data == null) return NotFound();
            return Ok(data);
        }

        // POST /Admin/Api/EmployeeAccountApi
        [HttpPost]
        public async Task<ActionResult<CreateEmployeeWithAccountResp>> Create([FromBody] CreateEmployeeReq req, CancellationToken ct)
        {
            var result = await _create.CreateEmployeeWithAccount(req, ct);
            return CreatedAtAction(
               nameof(GetOne),
               new { employeeId = result.EmployeeId, accountId = result.AccountId },
               result
           );
        }

        // PUT /Admin/Api/EmployeeAccountApi/{id}
        [HttpPut("{employeeId:guid}/{accountId:guid}")]
        public async Task<ActionResult<UpdateResult>> Update([FromRoute] Guid employeeId, [FromRoute] Guid accountId, [FromBody] UpdateEmployeeAccountReq req, CancellationToken ct)
        {
            // 防止隨便改 body 的 id
            req.EmployeeId = employeeId;
            req.AccountId = accountId;

            var result = await _update.UpdateEmployeeAccountAsync(req, ct);
            return result switch
            {
                UpdateResult.NotFound => NotFound(),
                UpdateResult.Updated => NoContent(),
                UpdateResult.NoChange => NoContent(),
                _ => StatusCode(500)
            };
        }

        // PATCH /Admin/Api/EmployeeAccountApi/{id}/status
        [HttpPatch("{employeeId:guid}/{accountId:guid}/status")]
        public async Task<ActionResult<UpdateResult>> UpdateStatus([FromRoute] Guid employeeId, [FromRoute] Guid accountId, [FromBody] UpdateStatusBody body, CancellationToken ct)
        {
            var req = new UpdateEmployeeAccountActiveReq
            {
                EmployeeId = employeeId,
                AccountId = accountId,
                EmployeeStatus = body.Status,
                AccountStatus = body.Status,
            };

            var result = await _update.UpdateEmployeeAccountActiveAsync(req, ct);
            return result switch
            {
                UpdateResult.NotFound => NotFound(),
                UpdateResult.Updated => NoContent(),
                UpdateResult.NoChange => NoContent(),
                _ => StatusCode(500)
            };
        }
    }
}
