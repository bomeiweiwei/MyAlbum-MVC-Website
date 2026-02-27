using Microsoft.AspNetCore.Identity;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using MyAlbum.Models.EmployeeAccount;
using MyAlbum.Models.Identity;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.Idenyity;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace MyAlbum.Application.EmployeeAccount.implement
{
    public class EmployeeAccountCreateService : BaseService, IEmployeeAccountCreateService
    {
        private readonly IPasswordHasher<AccountDto> _hasher;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IEmployeeAccountCreateRepository _employeeAccountCreateRepository;
        public EmployeeAccountCreateService(
           IAlbumDbContextFactory factory,
           IPasswordHasher<AccountDto> hasher,
           ICurrentUserAccessor currentUser,
           IEmployeeAccountCreateRepository employeeAccountCreateRepository) : base(factory)
        {
            _hasher = hasher;
            _currentUser = currentUser;
            _employeeAccountCreateRepository = employeeAccountCreateRepository;
        }

        public async Task<CreateEmployeeWithAccountResp> CreateEmployeeWithAccount(CreateEmployeeReq req, CancellationToken ct = default)
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
            EmployeeCreateDto employeeCreateDto = new EmployeeCreateDto
            {
                EmployeeId = Guid.NewGuid(),
                AccountId = accountCreateDto.AccountId,
                Email = req.Email,
                Phone = req.Phone,
                CreatedBy = operatorId
            };

            return await _employeeAccountCreateRepository.CreateEmployeeWithAccountAsync(accountCreateDto, employeeCreateDto, ct);
        }
    }
}
