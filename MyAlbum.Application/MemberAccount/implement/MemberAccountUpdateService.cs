using Microsoft.AspNetCore.Identity;
using MyAlbum.Application.Member;
using MyAlbum.Application.Uploads;
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
using MyAlbum.Models.UploadFiles;
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
        private readonly IFileUploadManagerService _fileUploadManagerService;
        public MemberAccountUpdateService(
           IAlbumDbContextFactory factory,
           IExecutionStrategyFactory strategyFactory,
           IPasswordHasher<AccountDto> hasher,
           ICurrentUserAccessor currentUser,
           IEmployeeAccountCreateRepository employeeAccountCreateRepository,
           IAccountUpdateRepository accountUpdateRepository,
           IMemberUpdateRepository memberUpdateRepository,
           IMemberAvatarUploadService memberDataUploadService,
           IFileUploadManagerService fileUploadManagerService
           ) : base(factory)
        {
            _strategyFactory = strategyFactory;
            _hasher = hasher;
            _currentUser = currentUser;
            _accountUpdateRepository = accountUpdateRepository;
            _memberUpdateRepository = memberUpdateRepository;
            _fileUploadManagerService = fileUploadManagerService;
        }

        public async Task<bool> UpdateMemberAccountAsync(UpdateMemberAccountReq req, IReadOnlyList<UploadFileStream> files, CancellationToken ct = default)
        {
            var result = false;
            var operatorId = _currentUser.GetRequiredAccountId();
            var passwordHash = "";
            if (!string.IsNullOrWhiteSpace(req.Password))
            {
                passwordHash = _hasher.HashPassword(null!, req.Password);
            }

            // 上傳檔案並取得檔案位置
            Dictionary<string, string> dict = new Dictionary<string, string>();
            int fileCount = files.Count;
            if (fileCount > 0)
            {
                UploadModel uploadModel = new UploadModel()
                {
                    Id = req.MemberId,
                    ColumnType = 1,
                    EntityUploadType = EntityUploadType.Member
                };
                dict = await _fileUploadManagerService.FileUpload(uploadModel, files, ct);
            }
            string avatarPath = string.Empty;
            if (dict.Count > 0)
            {
                var chkFile = files.First();
                if (dict.TryGetValue(chkFile.FileName, out var name))
                {
                    avatarPath = name;
                }
            }

            var now = DateTime.UtcNow;
            AccountUpdateDto accountDto = new AccountUpdateDto
            {
                AccountId = req.AccountId,
                PasswordHash = passwordHash,
                Status = req.Status,
                UpdatedAtUtc = now,
                UpdateBy = operatorId
            };

            MemberUpdateDto memberDto = new MemberUpdateDto
            {
                MemberId = req.MemberId,
                AccountId = req.AccountId,
                Email = req.Email,
                DisplayName = req.DisplayName,
                AvatarPath = !string.IsNullOrWhiteSpace(avatarPath) ? avatarPath : null,
                Status = req.Status,
                UpdatedAtUtc = now,
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
                    var memberResult = await _memberUpdateRepository.UpdateMemberAsync(ctx, memberDto, ct);

                    result = accountResult== UpdateResult.Updated && memberResult;
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

            var now = DateTime.UtcNow;
            AccountUpdateDto accountDto = new AccountUpdateDto
            {
                AccountId = req.AccountId,
                Status = req.Status,
                UpdatedAtUtc = now,
                UpdateBy = operatorId
            };

            MemberUpdateDto memberDto = new MemberUpdateDto
            {
                MemberId = req.MemberId,
                AccountId = accountDto.AccountId,
                Status = req.Status,
                UpdatedAtUtc = now,
                UpdateBy = operatorId
            };

            using var ctx = MainDB(ConnectionMode.Master);
            var strategy = _strategyFactory.Create(ctx);

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await ctx.BeginTransactionAsync(ct);

                try
                {
                    var accountResult = await _accountUpdateRepository.UpdateAccountActiveAsync(ctx, accountDto, ct);
                    var memberResult = await _memberUpdateRepository.UpdateMemberActiveAsync(ctx, memberDto, ct);

                    result = accountResult== UpdateResult.Updated && memberResult;
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
