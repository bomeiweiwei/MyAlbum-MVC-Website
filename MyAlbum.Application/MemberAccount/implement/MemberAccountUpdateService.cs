using Microsoft.AspNetCore.Identity;
using MyAlbum.Domain;
using MyAlbum.Domain.Account;
using MyAlbum.Domain.Employee;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Domain.Member;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using MyAlbum.Models.Identity;
using MyAlbum.Models.Member;
using MyAlbum.Models.MemberAccount;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.Idenyity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.MemberAccount.implement
{
    public class MemberAccountUpdateService : BaseService, IMemberAccountUpdateService
    {
        private readonly IExecutionStrategyFactory _strategyFactory;
        private readonly IPasswordHasher<AccountDto> _hasher;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IAccountUpdateRepository _accountUpdateRepository;
        private readonly IMemberUpdateRepository _memberUpdateRepository;
        public MemberAccountUpdateService(
           IAlbumDbContextFactory factory,
           IExecutionStrategyFactory strategyFactory,
           IPasswordHasher<AccountDto> hasher,
           ICurrentUserAccessor currentUser,
           IEmployeeAccountCreateRepository employeeAccountCreateRepository,
           IAccountUpdateRepository accountUpdateRepository,
           IMemberUpdateRepository memberUpdateRepository) : base(factory)
        {
            _strategyFactory = strategyFactory;
            _hasher = hasher;
            _currentUser = currentUser;
            _accountUpdateRepository = accountUpdateRepository;
            _memberUpdateRepository = memberUpdateRepository;
        }

        public async Task<bool> UpdateMemberAccountAsync(UpdateMemberAccountReq req, CancellationToken ct = default)
        {
            var result = false;
            var operatorId = _currentUser.GetRequiredAccountId();
            var passwordHash = "";
            if (!string.IsNullOrWhiteSpace(req.Password))
            {
                passwordHash = _hasher.HashPassword(null!, req.Password);
            }

            AccountUpdateDto accountDto = new AccountUpdateDto
            {
                AccountId = req.AccountId,
                PasswordHash = passwordHash,
                Status = req.AccountStatus,
                UpdateBy = operatorId
            };

            MemberUpdateDto employeeDto = new MemberUpdateDto
            {
                MemberId = req.MemberId,
                AccountId = req.AccountId,
                Email = req.Email,
                DisplayName = req.DisplayName,
                Status = req.MemberStatus,
                UpdateBy = operatorId
            };

            using var ctx = MainDB(ConnectionMode.Master);
            var strategy = _strategyFactory.Create(ctx);

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await ctx.BeginTransactionAsync(ct);

                try
                {
                    var accountResult = await _accountUpdateRepository.UpdateAccountAsync(ctx, accountDto, ct);
                    var memberResult = await _memberUpdateRepository.UpdateMemberAsync(ctx, employeeDto, ct);

                    result = accountResult && memberResult;
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

        public async Task<bool> UpdateMemberAccountActiveAsync(UpdateMemberAccountActiveReq req, CancellationToken ct = default)
        {
            var result = false;
            var operatorId = _currentUser.GetRequiredAccountId();
            AccountUpdateDto accountDto = new AccountUpdateDto
            {
                AccountId = req.AccountId,
                Status = req.AccountStatus,
                UpdateBy = operatorId
            };

            MemberUpdateDto employeeDto = new MemberUpdateDto
            {
                MemberId = req.MemberId,
                AccountId = req.AccountId,
                Status = req.MemberStatus,
                UpdateBy = operatorId
            };

            using var ctx = MainDB(ConnectionMode.Master);
            var strategy = _strategyFactory.Create(ctx);

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await ctx.BeginTransactionAsync(ct);

                try
                {
                    var accountResult = await _accountUpdateRepository.UpdateAccountAsync(ctx, accountDto, ct);
                    var memberResult = await _memberUpdateRepository.UpdateMemberAsync(ctx, employeeDto, ct);

                    result = accountResult && memberResult;
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

        
    }
}
