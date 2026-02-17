using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.AlbumComment;
using MyAlbum.Application.AlbumPhoto;
using MyAlbum.Application.Category;
using MyAlbum.Application.Member;
using MyAlbum.Application.Member.implement;
using MyAlbum.Application.MemberAccount;
using MyAlbum.Models.AlbumComment;
using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Models.Base;
using MyAlbum.Models.Category;
using MyAlbum.Models.MemberAccount;
using MyAlbum.Models.UploadFiles;
using MyAlbum.Shared.Extensions;
using MyAlbum.Web.Models;
using MyAlbum.Web.Models.AlbumCategory;
using MyAlbum.Web.Models.AlbumComment;
using MyAlbum.Web.Models.TopImgComment;
using System.Diagnostics;

namespace MyAlbum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemberRegisterService _memberRegisterService;
        private readonly ITopPhotoService _topPhotoService;
        private readonly ICategoryPhotoReadService _categoryPhotoReadService;
        private readonly IAlbumPhotoCommentReadService _albumPhotoCommentReadService;
        private readonly IAlbumCommentCreateService _albumCommentCreateService;
        private readonly IMemberAccountReadService _memberAccountReadService;
        public HomeController(
            IMemberRegisterService memberRegisterService,
            ITopPhotoService topPhotoService,
            ICategoryPhotoReadService categoryPhotoReadService,
            IAlbumPhotoCommentReadService albumPhotoCommentReadService,
            IAlbumCommentCreateService albumCommentCreateService,
            IMemberAccountReadService memberAccountReadService
            )
        {
            _memberRegisterService = memberRegisterService;
            _topPhotoService = topPhotoService;
            _categoryPhotoReadService = categoryPhotoReadService;
            _albumPhotoCommentReadService = albumPhotoCommentReadService;
            _albumCommentCreateService = albumCommentCreateService;
            _memberAccountReadService = memberAccountReadService;
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

        public async Task<IActionResult> CommentList(Guid albumPhotoId)
        {
            var data = await _albumPhotoCommentReadService.GetAlbumPhotoComments(albumPhotoId);
            if (data.Photo == null)
                return NotFound();

            AlbumPhotoCommentsViewModel vm = new AlbumPhotoCommentsViewModel(data.Photo, data.Comments);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(AddAlbumCommentReq req)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(req.Comment) || req.Comment.Trim() == "")
                {
                    TempData["Error"] = "留言內容不能為空";
                    return RedirectToAction("CommentList", new { albumPhotoId = req.AlbumPhotoId });
                }

                if (User.Identity?.IsAuthenticated == true)
                {
                    var loginAccountId = User.GetAccountId();
                    if (!loginAccountId.HasValue)
                    {
                        TempData["Error"] = "登入失敗";
                        return RedirectToAction("CommentList", new { albumPhotoId = req.AlbumPhotoId });
                    }

                    GetMemberAccountReq getMemberAccountReq = new GetMemberAccountReq()
                    {
                        AccountId = loginAccountId.Value
                    };
                    var member = await _memberAccountReadService.GetMemberAccountAsync(getMemberAccountReq);
                    if (member == null)
                    {
                        TempData["Error"] = "找不到會員";
                        return RedirectToAction("CommentList", new { albumPhotoId = req.AlbumPhotoId });
                    }

                    CreateAlbumCommentReq createReq = new CreateAlbumCommentReq()
                    {
                        AlbumPhotoId = req.AlbumPhotoId,
                        MemberId = member.MemberId,
                        Comment = req.Comment
                    };
                    await _albumCommentCreateService.CreateAlbumCommentAsync(createReq);
                }
            }
            return RedirectToAction("CommentList", new { albumPhotoId = req.AlbumPhotoId });
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
