using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Editing;
using MyAlbum.Application.EmployeeAccount;
using MyAlbum.Application.Member;
using MyAlbum.Application.Test;
using MyAlbum.Infrastructure.EF.Models;
using MyAlbum.Models.EmployeeAccount;

namespace MyAlbum.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public TestController(
            IWebHostEnvironment env
            )
        {
            _env = env;
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
    }
}
