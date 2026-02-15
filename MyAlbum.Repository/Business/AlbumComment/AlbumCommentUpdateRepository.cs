using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.AlbumComment;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.AlbumComment;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository.Business.AlbumComment
{
    public sealed class AlbumCommentUpdateRepository : IAlbumCommentUpdateRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public AlbumCommentUpdateRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<bool> UpdateAlbumCommentAsync(AlbumCommentUpdateDto dto, CancellationToken ct = default)
        {
            bool result = false;

            using var ctx = _factory.Create(ConnectionMode.Master);
            var db = ctx.AsDbContext<MyAlbumContext>();
            var strategy = db.Database.CreateExecutionStrategy();

            /* 資料檢查區塊 */
            var data = await db.AlbumComments.FirstOrDefaultAsync(m => m.AlbumCommentId == dto.AlbumCommentId, ct);

            if (data == null)
                throw new InvalidOperationException("找不到指定的資料。");

            /* 額外確認 */
            if (dto.UpdateByMember && data.MemberId != dto.MemberId)
                throw new InvalidOperationException("非本人無法更新。");

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync(ct);
                try
                {
                    /* 設定資料區塊 */
                    data.Comment = dto.Comment;
                    data.IsChanged = dto.IsChanged;
                    data.Status = (byte)dto.Status;
                    data.UpdatedBy = dto.UpdatedBy;
                    data.UpdatedAtUtc = dto.UpdatedAtUtc;

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

        public async Task<bool> UpdateAlbumCommentActiveAsync(AlbumCommentUpdateActiveDto dto, CancellationToken ct = default)
        {
            bool result = false;

            using var ctx = _factory.Create(ConnectionMode.Master);
            var db = ctx.AsDbContext<MyAlbumContext>();
            var strategy = db.Database.CreateExecutionStrategy();

            /* 資料檢查區塊 */
            var data = await db.AlbumComments.FirstOrDefaultAsync(m =>  m.AlbumCommentId == dto.AlbumCommentId, ct);

            if (data == null)
                throw new InvalidOperationException("找不到指定的資料。");

            /* 額外確認 */
            if (dto.UpdateByMember && data.MemberId != dto.MemberId)
                throw new InvalidOperationException("非本人無法更新。");

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync(ct);
                try
                {
                    /* 設定資料區塊 */
                    data.Status = (byte)dto.Status;
                    data.UpdatedBy = dto.UpdatedBy;
                    data.UpdatedAtUtc = dto.UpdatedAtUtc;

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
