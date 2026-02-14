using Microsoft.AspNetCore.Identity;
using MyAlbum.Application.Member;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Domain.MemberAccount;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using MyAlbum.Models.Identity;
using MyAlbum.Models.Member;
using MyAlbum.Models.MemberAccount;
using MyAlbum.Models.UploadFiles;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.Idenyity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyAlbum.Application.MemberAccount.implement
{
    public class MemberAccountCreateService : BaseService, IMemberAccountCreateService
    {
        private readonly IPasswordHasher<AccountDto> _hasher;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IMemberAccountCreateRepository _memberAccountCreateRepository;
        private readonly IMemberDataUploadService _memberDataUploadService;
        public MemberAccountCreateService(
           IAlbumDbContextFactory factory,
           IPasswordHasher<AccountDto> hasher,
           ICurrentUserAccessor currentUser,
           IMemberAccountCreateRepository memberAccountCreateRepository,
           IMemberDataUploadService memberDataUploadService) : base(factory)
        {
            _hasher = hasher;
            _currentUser = currentUser;
            _memberAccountCreateRepository = memberAccountCreateRepository;
            _memberDataUploadService = memberDataUploadService;
        }

        public async Task<Guid> CreateMemberWithAccountAsync(CreateMemberReq req, IReadOnlyList<UploadFileStream> files, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            var passwordHash = _hasher.HashPassword(null!, req.Password);

            Guid accountId = Guid.NewGuid();
            Guid memberId = Guid.NewGuid();

            DateTime now = DateTime.UtcNow;

            AccountCreateDto accountCreateDto = new AccountCreateDto
            {
                AccountId = accountId,
                UserName = req.UserName,
                PasswordHash = passwordHash,
                CreatedAtUtc = now,
                CreatedBy = operatorId
            };
            MemberCreateDto memberCreateDto = new MemberCreateDto
            {
                MemberId = memberId,
                AccountId = accountCreateDto.AccountId,
                Email = req.Email,
                DisplayName = req.DisplayName,
                CreatedAtUtc = now,
                CreatedBy = operatorId
            };

            var id = await _memberAccountCreateRepository.CreateMemberWithAccountAsync(accountCreateDto, memberCreateDto, ct);

            // 上傳檔案並取得檔案位置
            int fileCount = files.Count;
            if (fileCount > 0)
            {
                var f = files.First();
                var avatarPath = await _memberDataUploadService.UploadAvatarAsync(id, f.Stream, f.FileName, Mode.Create, ct);
            }

            return id;
        }
    }
}
