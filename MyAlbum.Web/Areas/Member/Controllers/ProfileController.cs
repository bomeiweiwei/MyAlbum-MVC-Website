using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using MyAlbum.Application.MemberAccount;
using MyAlbum.Models.MemberAccount;
using MyAlbum.Models.UploadFiles;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.Idenyity;
using MyAlbum.Web.Models.MemberAccount;

namespace MyAlbum.Web.Areas.Member.Controllers
{
    [Area("Member")]
    public class ProfileController : Controller
    {
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IMemberAccountReadService _memberAccountReadService;
        private readonly IMemberAccountUpdateService _memberAccountUpdateService;
        public ProfileController(
            ICurrentUserAccessor currentUser,
            IMemberAccountReadService memberAccountReadService,
            IMemberAccountUpdateService memberAccountUpdateService
            ) 
        {
            _currentUser = currentUser;
            _memberAccountReadService = memberAccountReadService;
            _memberAccountUpdateService = memberAccountUpdateService;
        }

        public async Task<IActionResult> Index()
        {
            var accountId = _currentUser.GetRequiredAccountId();
            GetMemberAccountReq req = new GetMemberAccountReq
            {
                AccountId = accountId
            };
            var data = await _memberAccountReadService.GetMemberAccountAsync(req);
            if (data == null)
                return NotFound();

            return View(data);
        }

        public async Task<IActionResult> Edit(Guid memberId, CancellationToken ct)
        {
            var accountId = _currentUser.GetRequiredAccountId();

            GetMemberAccountReq req = new GetMemberAccountReq
            {
                AccountId = accountId,
                MemberId = memberId
            };

            var data = await _memberAccountReadService.GetMemberAccountAsync(req);
            if (data == null)
                return Forbid();

            EditMemberViewModel vm= new EditMemberViewModel
            {
                AccountId = data.AccountId,
                MemberId = data.MemberId,
                DisplayName = data.DisplayName,
                Email = data.Email,
                PublicAvatarUrl = data.PublicAvatarUrl,
                Avatar = data.PublicAvatarUrl
            };

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditMemberViewModel model, IFormFile? File)
        {
            var accountId = _currentUser.GetRequiredAccountId();
            if (model.AccountId != accountId)
                return Forbid();
            // 1) 組更新 req（不含檔案）
            UpdateMemberAccountReq req = new UpdateMemberAccountReq
            {
                AccountId = model.AccountId,
                MemberId = model.MemberId,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                DisplayName = model.DisplayName,
                Email = model.Email,
                Status = Status.Active
            };
            // 2) 有選檔才處理檔案更新
            IReadOnlyList<UploadFileStream>? files = null;
            if (File is not null && File.Length > 0)
            {
                // 3) 檔案規則
                const long maxSize = 2 * 1024 * 1024; // 2MB
                if (File.Length > maxSize)
                {
                    ModelState.AddModelError(nameof(model.Avatar), "檔案過大，限制 2MB");
                }

                var allowedExt = new[] { ".jpg", ".jpeg", ".png" };
                var ext = Path.GetExtension(File.FileName).ToLowerInvariant();
                if (!allowedExt.Contains(ext))
                {
                    ModelState.AddModelError(nameof(model.Avatar), "僅允許 jpg / jpeg / png");
                }

                var allowedContentTypes = new[] { "image/jpeg", "image/png" };
                if (!allowedContentTypes.Contains(File.ContentType))
                {
                    ModelState.AddModelError(nameof(model.Avatar), "檔案格式不正確");
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                await using var stream = File.OpenReadStream();
                files = new List<UploadFileStream>
                {
                    new UploadFileStream
                    {
                        FileName = File.FileName,
                        ContentType = File.ContentType,
                        Length = File.Length,
                        Stream = stream
                    }
                };
                await _memberAccountUpdateService.UpdateMemberAccountAsync(req, files);
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                IReadOnlyList<UploadFileStream> emptyfiles = new List<UploadFileStream>();
                await _memberAccountUpdateService.UpdateMemberAccountAsync(req, emptyfiles);
            }

            return RedirectToAction("Index");
        }
    }
}
