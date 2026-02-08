using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.Member;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.Identity;
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
    }
}
