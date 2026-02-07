using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.EmployeeAccount;
using MyAlbum.Application.Test;
using MyAlbum.Models.EmployeeAccount;

namespace MyAlbum.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ITestService _testService;
        private readonly IEmployeeAccountCreateService _employeeAccountCreateService;
        private readonly IEmployeeAccountReadService _employeeAccountReadService;
        public TestController(
            IWebHostEnvironment env
            , ITestService testService
            , IEmployeeAccountCreateService employeeAccountCreateService
            , IEmployeeAccountReadService employeeAccountReadService
            )
        {
            _env = env;
            _testService = testService;
            _employeeAccountCreateService = employeeAccountCreateService;
            _employeeAccountReadService = employeeAccountReadService;
        }
        /// <summary>
        /// 取得環境變數
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEnv")]
        public async Task<ActionResult<string>> GetEnv()
        {
            var environment = _env.EnvironmentName;
            return Ok(environment);
        }
        /// <summary>
        /// 檢查資料庫連線
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetConnectResult")]
        public async Task<ActionResult<bool>> GetConnectResult()
        {
            var connectResult = await _testService.GetConnectResult();
            return Ok(connectResult);
        }
        /// <summary>
        /// 測試員工新增
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("CreateEmployeeWithAccount")]
        [Authorize(AuthenticationSchemes = "AdminAuth")]
        public async Task<ActionResult<bool>> CreateEmployeeWithAccount([FromBody]CreateEmployeeReq req, CancellationToken ct = default)
        {
            Guid result = await _employeeAccountCreateService.CreateEmployeeWithAccount(req, ct);
            if (result != Guid.Empty)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }

        [HttpGet("GetEmployeeAccount")]
        public async Task<ActionResult<EmployeeAccountDto>> GetEmployeeAccount([FromQuery] GetEmployeeAccountReq req, CancellationToken ct = default)
        {
            var result = await _employeeAccountReadService.GetEmployeeAccountAsync(req, ct);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("GetEmployeeAccountList")]
        public async Task<ActionResult<List<EmployeeAccountDto>>> GetEmployeeAccountList([FromQuery] GetEmployeeAccountListReq req, CancellationToken ct = default)
        {
            var result = await _employeeAccountReadService.GetEmployeeAccountListAsync(req, ct);
            return Ok(result);
        }
    }
}
