using Microsoft.AspNetCore.Identity;
using MyAlbum.Application.MemberAccount;
using MyAlbum.Domain;
using MyAlbum.Domain.MemberAccount;
using MyAlbum.Models.Account;
using MyAlbum.Models.Identity;
using MyAlbum.Models.Member;
using MyAlbum.Models.MemberAccount;
using MyAlbum.Models.UploadFiles;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.WebRequestMethods;

namespace MyAlbum.Application.Member.implement
{
    public class MemberRegisterService : BaseService, IMemberRegisterService
    {
        private readonly IPasswordHasher<AccountDto> _hasher;
        private readonly IMemberAccountCreateRepository _create;
        public MemberRegisterService(
            IAlbumDbContextFactory factory,
            IPasswordHasher<AccountDto> hasher,
            IMemberAccountCreateRepository create) : base(factory)
        {
            _hasher = hasher;
            _create = create;
        }

        public async Task<Guid> Register(CreateMemberReq req, CancellationToken ct = default)
        {
            var operatorId = Guid.Empty;
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
                AvatarPath =  null,
                DisplayName = req.DisplayName,
                CreatedAtUtc = now,
                CreatedBy = operatorId
            };

            var id = await _create.CreateMemberWithAccountAsync(accountCreateDto, memberCreateDto, ct);
            return id;
        }
    }
}
