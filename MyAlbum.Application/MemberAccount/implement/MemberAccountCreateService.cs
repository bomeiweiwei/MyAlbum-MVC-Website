using Microsoft.AspNetCore.Identity;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Domain.MemberAccount;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using MyAlbum.Models.Identity;
using MyAlbum.Models.Member;
using MyAlbum.Models.MemberAccount;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.Idenyity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.MemberAccount.implement
{
    public class MemberAccountCreateService : BaseService, IMemberAccountCreateService
    {
        private readonly IPasswordHasher<AccountDto> _hasher;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IMemberAccountCreateRepository _memberAccountCreateRepository;
        public MemberAccountCreateService(
           IAlbumDbContextFactory factory,
           IPasswordHasher<AccountDto> hasher,
           ICurrentUserAccessor currentUser,
           IMemberAccountCreateRepository memberAccountCreateRepository) : base(factory)
        {
            _hasher = hasher;
            _currentUser = currentUser;
            _memberAccountCreateRepository = memberAccountCreateRepository;
        }

        public async Task<Guid> CreateMemberWithAccountAsync(CreateMemberReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            var passwordHash = _hasher.HashPassword(null!, req.Password);

            AccountCreateDto accountCreateDto = new AccountCreateDto
            {
                AccountId = Guid.NewGuid(),
                UserName = req.UserName,
                PasswordHash = passwordHash,
                CreatedBy = operatorId
            };
            MemberCreateDto memberCreateDto = new MemberCreateDto
            {
                MemberId = Guid.NewGuid(),
                AccountId = accountCreateDto.AccountId,
                Email = req.Email,
                DisplayName = req.DisplayName,
                CreatedBy = operatorId
            };

            return await _memberAccountCreateRepository.CreateMemberWithAccountAsync(accountCreateDto, memberCreateDto, ct);
        }
    }
}
