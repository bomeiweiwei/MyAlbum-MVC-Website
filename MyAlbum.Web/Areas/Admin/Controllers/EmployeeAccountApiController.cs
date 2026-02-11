using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.Category;
using MyAlbum.Application.EmployeeAccount;
using MyAlbum.Models.Base;
using MyAlbum.Models.Category;
using MyAlbum.Models.EmployeeAccount;

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
        public async Task<ActionResult<Guid>> Create([FromBody] CreateEmployeeReq req, CancellationToken ct)
        {
            var id = await _create.CreateEmployeeWithAccount(req, ct);
            return Ok(id);
        }

        // PUT /Admin/Api/EmployeeAccountApi/{id}
        [HttpPut("{employeeId:guid}/{accountId:guid}")]
        public async Task<ActionResult<bool>> Update([FromRoute] Guid employeeId, [FromRoute] Guid accountId, [FromBody] UpdateEmployeeAccountReq req, CancellationToken ct)
        {
            // 防止隨便改 body 的 id
            req.EmployeeId = employeeId;
            req.AccountId = accountId;

            var ok = await _update.UpdateEmployeeAccountAsync(req, ct);
            return Ok(ok);
        }

        // PATCH /Admin/Api/EmployeeAccountApi/{id}/status
        [HttpPatch("{employeeId:guid}/{accountId:guid}/status")]
        public async Task<ActionResult<bool>> UpdateStatus([FromRoute] Guid employeeId, [FromRoute] Guid accountId, [FromBody] UpdateStatusBody body, CancellationToken ct)
        {
            var req = new UpdateEmployeeAccountActiveReq
            {
                EmployeeId = employeeId,
                AccountId = accountId,
                EmployeeStatus = body.Status,
                AccountStatus = body.Status,
            };

            var ok = await _update.UpdateEmployeeAccountActiveAsync(req, ct);
            return Ok(ok);
        }
    }
}
