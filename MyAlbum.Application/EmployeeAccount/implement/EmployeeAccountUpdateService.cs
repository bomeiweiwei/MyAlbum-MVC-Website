using Microsoft.AspNetCore.Identity;
using MyAlbum.Domain;
using MyAlbum.Domain.Account;
using MyAlbum.Domain.Employee;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using MyAlbum.Models.EmployeeAccount;
using MyAlbum.Models.Identity;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.Idenyity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.EmployeeAccount.implement
{
    public class EmployeeAccountUpdateService : BaseService, IEmployeeAccountUpdateService
    {
        private readonly IExecutionStrategyFactory _strategyFactory;
        private readonly IPasswordHasher<AccountDto> _hasher;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IAccountUpdateRepository _accountUpdateRepository;
        private readonly IEmployeeUpdateRepository _employeeUpdateRepository;
        public EmployeeAccountUpdateService(
           IAlbumDbContextFactory factory,
           IExecutionStrategyFactory strategyFactory,
           IPasswordHasher<AccountDto> hasher,
           ICurrentUserAccessor currentUser,
           IEmployeeAccountCreateRepository employeeAccountCreateRepository,
           IAccountUpdateRepository accountUpdateRepository,
           IEmployeeUpdateRepository employeeUpdateRepository) : base(factory)
        {
            _strategyFactory = strategyFactory;
            _hasher = hasher;
            _currentUser = currentUser;
            _accountUpdateRepository = accountUpdateRepository;
            _employeeUpdateRepository = employeeUpdateRepository;
        }

        public async Task<bool> UpdateEmployeeAccountAsync(UpdateEmployeeAccountReq req, CancellationToken ct = default)
        {
            var result = false;
            var id = _currentUser.GetRequiredAccountId();
            var passwordHash = "";
            if (!string.IsNullOrWhiteSpace(req.Password))
            {
                passwordHash = _hasher.HashPassword(null!, req.Password);
            }

            AccountUpdateDto accountDto = new AccountUpdateDto
            {
                AccountId = id,
                PasswordHash = passwordHash,
                Status = req.AccountStatus,
                UpdateBy = id
            };

            EmployeeUpdateDto employeeDto = new EmployeeUpdateDto
            {
                EmployeeId = req.EmployeeId,
                AccountId = req.AccountId,
                Email = req.Email,
                Phone = req.Phone,
                Status = req.EmployeeStatus,
                UpdateBy = id
            };

            using var ctx = MainDB(ConnectionMode.Master);
            var strategy = _strategyFactory.Create(ctx);

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await ctx.BeginTransactionAsync(ct);

                try
                {
                    var accountResult = await _accountUpdateRepository.UpdateAccountAsync(ctx, accountDto, ct);
                    var employeeResult = await _employeeUpdateRepository.UpdateEmployeeAsync(ctx, employeeDto, ct);

                    result = accountResult && employeeResult;
                    if (result)
                        await tx.CommitAsync(ct);
                    else
                        await tx.RollbackAsync(ct);
                }
                catch
                {
                    await tx.RollbackAsync(ct);
                    throw;
                }
            }, ct);

            return result;
        }

        public async Task<bool> UpdateEmployeeAccountActiveeAsync(UpdateEmployeeAccountActiveReq req, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
