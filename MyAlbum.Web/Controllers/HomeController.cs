using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.Member;
using MyAlbum.Application.Member.implement;
using MyAlbum.Application.MemberAccount;
using MyAlbum.Models.MemberAccount;
using MyAlbum.Models.UploadFiles;
using MyAlbum.Web.Models;
using System.Diagnostics;

namespace MyAlbum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemberRegisterService _memberRegisterService;
        public HomeController(IMemberRegisterService memberRegisterService)
        {
            _memberRegisterService = memberRegisterService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(CreateMemberReq req)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Msg = "";
                try
                { 
                    Guid resp = await _memberRegisterService.Register(req);
                    return RedirectToAction(
                         actionName: "Login",
                         controllerName: "Identity",
                         routeValues: new { area = "Member" }
                     );
                }
                catch (Exception ex)
                {
                    ViewBag.Msg = "會員註冊失敗，帳號可能重複";
                }
            }
            return View(req);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
