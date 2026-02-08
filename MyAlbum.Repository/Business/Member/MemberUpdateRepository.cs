using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.Member;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.Identity;
using MyAlbum.Models.Member;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository.Business.Member
{
    public class MemberUpdateRepository : IMemberUpdateRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public MemberUpdateRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<bool> UpdateMemberAvatarPathAsync(Guid memberId, string fileKey, Guid operatorId, CancellationToken ct)
        {
            var result = false;

            using var ctx = _factory.Create(ConnectionMode.Master);
            var db = ctx.AsDbContext<MyAlbumContext>();
            var strategy = db.Database.CreateExecutionStrategy();

            var member = await db.Members
                .FirstOrDefaultAsync(m => m.MemberId == memberId, ct);

            if (member == null)
            {
                throw new InvalidOperationException("會員不存在。");
            }

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync(ct);
                try
                {
                    DateTime now = DateTime.UtcNow;

                    member.AvatarPath = fileKey;
                    member.UpdatedBy = operatorId;

                    await db.SaveChangesAsync(ct);

                    await tx.CommitAsync(ct);

                    result = true;
                }
                catch
                {
                    await tx.RollbackAsync(ct);
                    throw;
                }
            });

            return result;
        }

        public async Task<bool> UpdateMemberAsync(IAlbumDbContext ctx, MemberUpdateDto memberDto, CancellationToken ct = default)
        {
            var result = false;

            var db = ctx.AsDbContext<MyAlbumContext>();
            var data = await db.Members.Where(x => x.MemberId == memberDto.MemberId && x.AccountId == memberDto.AccountId).FirstOrDefaultAsync(ct);
            if (data == null)
                return result;

            data.Email = memberDto.Email;
            data.DisplayName = memberDto.DisplayName;
            data.Status = (byte)memberDto.Status;
            data.UpdatedBy = memberDto.UpdateBy;
            data.UpdatedAtUtc = DateTime.UtcNow;
            int check = await ctx.SaveChangesAsync(ct);
            if (check == 1)
                result = true;
            return result;
        }

        public async Task<bool> UpdateMemberActiveAsync(IAlbumDbContext ctx, MemberUpdateDto memberDto, CancellationToken ct = default)
        {
            var result = false;

            var db = ctx.AsDbContext<MyAlbumContext>();
            var data = await db.Employees.Where(x => x.EmployeeId == memberDto.MemberId && x.AccountId == memberDto.AccountId).FirstOrDefaultAsync(ct);
            if (data == null)
                return result;

            data.Status = (byte)memberDto.Status;
            data.UpdatedBy = memberDto.UpdateBy;
            data.UpdatedAtUtc = DateTime.UtcNow;
            int check = await ctx.SaveChangesAsync(ct);
            if (check == 1)
                result = true;
            return result;
        }
    }
}
