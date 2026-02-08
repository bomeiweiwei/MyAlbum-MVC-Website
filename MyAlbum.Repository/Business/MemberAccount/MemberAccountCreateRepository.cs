using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.MemberAccount;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using MyAlbum.Models.Member;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository.Business.MemberAccount
{
    public class MemberAccountCreateRepository : IMemberAccountCreateRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public MemberAccountCreateRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<Guid> CreateMemberWithAccountAsync(AccountCreateDto accountDto, MemberCreateDto memberDto, CancellationToken ct = default)
        {
            Guid result = Guid.Empty;

            using var ctx = _factory.Create(ConnectionMode.Master);
            var db = ctx.AsDbContext<MyAlbumContext>();
            var strategy = db.Database.CreateExecutionStrategy();


            var exists = await db.Accounts.AsNoTracking()
                .AnyAsync(a => a.UserName == accountDto.UserName, ct);

            if (exists)
            {
                throw new InvalidOperationException("帳號已存在。");
            }

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync(ct);
                try
                {
                    DateTime now = DateTime.UtcNow;

                    var account = new Infrastructure.EF.Models.Account
                    {
                        AccountId = accountDto.AccountId,
                        UserName = accountDto.UserName,
                        PasswordHash = accountDto.PasswordHash,
                        AccountType = (int)AccountType.Member,
                        Status = (int)Status.Active,
                        CreatedAtUtc = now,
                        CreatedBy = accountDto.CreatedBy,
                        UpdatedAtUtc = now,
                        UpdatedBy = accountDto.CreatedBy
                    };
                    await db.Accounts.AddAsync(account, ct);

                    var member = new Infrastructure.EF.Models.Member
                    {
                        MemberId = memberDto.MemberId,
                        AccountId = account.AccountId,
                        Email = memberDto.Email,
                        DisplayName = memberDto.DisplayName,
                        Status = (int)Status.Active,
                        CreatedAtUtc = now,
                        CreatedBy = memberDto.CreatedBy,
                        UpdatedAtUtc = now,
                        UpdatedBy = memberDto.CreatedBy
                    };
                    await db.Members.AddAsync(member, ct);

                    await db.SaveChangesAsync(ct);

                    await tx.CommitAsync(ct);

                    result = member.MemberId;
                }
                catch
                {
                    await tx.RollbackAsync(ct);
                    throw;
                }
            });

            return result;
        }
    }
}
