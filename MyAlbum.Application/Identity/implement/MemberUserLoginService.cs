using Microsoft.AspNetCore.Identity;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Domain.MemberAccount;
using MyAlbum.Models.Account;
using MyAlbum.Models.Identity;
using MyAlbum.Models.MemberAccount;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Identity.implement
{
    public class MemberUserLoginService : BaseService, ILoginService
    {
        public UserRole userRole => UserRole.Member;

        private readonly IPasswordHasher<AccountDto> _hasher;
        private readonly IMemberAccountReadRepository _memberAccountReadRepository;
        public MemberUserLoginService(IAlbumDbContextFactory factory, IPasswordHasher<AccountDto> hasher, IMemberAccountReadRepository memberAccountReadRepository) : base(factory)
        {
            _hasher = hasher;
            _memberAccountReadRepository = memberAccountReadRepository;
        }

        public async Task<AccountLoginResp> Login(LoginReq req, CancellationToken ct = default)
        {
            var result = new AccountLoginResp()
            {
                IsLoginSuccess = false,
                AccountType = AccountType.Member
            };

            GetMemAccountDto dto = new GetMemAccountDto()
            {
                UserName = req.UserName
            };

            AccountDto? account = await _memberAccountReadRepository.GeMemberByUserNameAsync(dto, ct);
            if (account == null)
            {
                return result;
            }

            var verify = _hasher.VerifyHashedPassword(account, account.PasswordHash, req.Password);
            if (verify == PasswordVerificationResult.Failed)
            {
                req.Password = string.Empty;
                account.PasswordHash = string.Empty;
                return result;
            }

            req.Password = string.Empty;
            account.PasswordHash = string.Empty;

            result.IsLoginSuccess = true;
            result.AccountId = account.AccountId;
            result.UserName = account.UserName;

            return result;
        }
    }
}
