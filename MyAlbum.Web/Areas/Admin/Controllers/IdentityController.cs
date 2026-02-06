using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.Identity;
using MyAlbum.Models.Identity;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.Extensions;
using MyAlbum.Web.Models.ViewModels;
using System.Security.Claims;

namespace MyAlbum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class IdentityController : Controller
    {
        private readonly ILoginManagerService _loginManagerService;
        public IdentityController(ILoginManagerService loginManagerService)
        {
            _loginManagerService = loginManagerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            LoginReq loginReq = new LoginReq()
            {
                UserName = model.UserName,
                Password = model.Password,
                Role = UserRole.Admin
            };

            var loginResult = await _loginManagerService.UserLogin(loginReq);

            if (!loginResult.IsLoginSuccess)
            {
                ModelState.AddModelError(string.Empty, "帳號或密碼錯誤。");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, loginResult.AccountId.ToString()),
                new Claim(ClaimTypes.Name, loginResult.UserName),
                new Claim("AccountType", AccountType.Admin.GetDescription())
            };

            // 依 UserType 選擇對應的 Cookie Scheme
            var scheme = "AdminAuth";
            var identity = new ClaimsIdentity(claims, scheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(scheme, principal, new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            });

            // 安全導回
            var fallbackUrl = Url.Action("Index", "Home", new { area = "Admin" });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return Redirect(fallbackUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            await HttpContext.SignOutAsync("AdminAuth"); // <- 後台 Cookie scheme
            Response.Cookies.Delete("adminAuth");

            // 回到後台登入頁或指定頁
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Login", "Identity", new { area = "Admin" });
        }
    }
}
