using Microsoft.AspNetCore.Identity;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Models.Account;
using MyAlbum.Models.EmployeeAccount;
using MyAlbum.Models.Identity;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Identity.implement
{
    public class AdminUserLoginService : BaseService, ILoginService
    {
        public UserRole userRole => UserRole.Admin;

        private readonly IPasswordHasher<AccountDto> _hasher;
        private readonly IEmployeeAccountReadRepository _employeeAccountReadRepository;
        public AdminUserLoginService(IAlbumDbContextFactory factory, IPasswordHasher<AccountDto> hasher, IEmployeeAccountReadRepository employeeAccountReadRepository) : base(factory)
        {
            _hasher = hasher;
            _employeeAccountReadRepository = employeeAccountReadRepository;
        }

        public async Task<AccountLoginResp> Login(LoginReq req, CancellationToken ct = default)
        {
            var result = new AccountLoginResp()
            {
                IsLoginSuccess = false,
                AccountType = AccountType.Admin
            };

            GetEmpAccountDto dto = new GetEmpAccountDto
            {
                UserName = req.UserName
            };

            AccountDto? account = await _employeeAccountReadRepository.GetEmployeeByUserNameAsync(dto, ct);
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
