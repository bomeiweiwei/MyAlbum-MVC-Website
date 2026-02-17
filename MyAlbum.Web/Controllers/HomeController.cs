using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.AlbumPhoto;
using MyAlbum.Application.Category;
using MyAlbum.Application.Member;
using MyAlbum.Application.Member.implement;
using MyAlbum.Application.MemberAccount;
using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Models.Base;
using MyAlbum.Models.Category;
using MyAlbum.Models.MemberAccount;
using MyAlbum.Models.UploadFiles;
using MyAlbum.Web.Models;
using MyAlbum.Web.Models.AlbumCategory;
using MyAlbum.Web.Models.TopImgComment;
using System.Diagnostics;

namespace MyAlbum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemberRegisterService _memberRegisterService;
        private readonly ITopPhotoService _topPhotoService;
        private readonly ICategoryPhotoReadService _categoryPhotoReadService;
        public HomeController(
            IMemberRegisterService memberRegisterService,
            ITopPhotoService topPhotoService,
            ICategoryPhotoReadService categoryPhotoReadService)
        {
            _memberRegisterService = memberRegisterService;
            _topPhotoService = topPhotoService;
            _categoryPhotoReadService = categoryPhotoReadService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _topPhotoService.GetTopPhotos();

            TopPhotoCommentViewModel model = new TopPhotoCommentViewModel();
            model.Photos = list;

            return View(model);
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


        public async Task<IActionResult> AlbumCategory(Guid albumCategoryId)
        {
            var data = await _categoryPhotoReadService.GetAlbumCategoryData(albumCategoryId);
            if (data == null || string.IsNullOrWhiteSpace(data.CategoryName))
            {
                return NotFound();
            }

            AlbumCategoryViewModel vm = new AlbumCategoryViewModel()
            {
                CategoryName = data.CategoryName,
                Photos = data.Photos
            };

            return View(vm);
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
