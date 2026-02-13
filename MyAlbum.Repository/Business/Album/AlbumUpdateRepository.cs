using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.Album;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Infrastructure.EF.Models;
using MyAlbum.Models.Album;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository.Business.Album
{
    public class AlbumUpdateRepository : IAlbumUpdateRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public AlbumUpdateRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<bool> UpdateAlbumAsync(AlbumUpdateDto dto, CancellationToken ct = default)
        {
            bool result = false;

            using var ctx = _factory.Create(ConnectionMode.Master);
            var db = ctx.AsDbContext<MyAlbumContext>();
            var strategy = db.Database.CreateExecutionStrategy();

            var data = await db.Albums.FirstOrDefaultAsync(m => m.AlbumId == dto.AlbumId);
            if (data == null)
                throw new InvalidOperationException("找不到指定的資料。");
            if (dto.UpdateByMember && data.OwnerAccountId != dto.OwnerAccountId)
                throw new InvalidOperationException("非本人無法更新。");
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync(ct);
                try
                {
                    data.AlbumCategoryId = dto.AlbumCategoryId;
                    data.OwnerAccountId = dto.OwnerAccountId;
                    data.Title = dto.Title;
                    data.Description = dto.Description;
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

        public async Task<bool> UpdateAlbumActiveAsync(AlbumUpdateActiveDto dto, CancellationToken ct = default)
        {
            bool result = false;

            using var ctx = _factory.Create(ConnectionMode.Master);
            var db = ctx.AsDbContext<MyAlbumContext>();
            var strategy = db.Database.CreateExecutionStrategy();

            var data = await db.Albums.FirstOrDefaultAsync(m => m.AlbumId == dto.AlbumId);
            if (data == null)
                throw new InvalidOperationException("找不到指定的資料。");
            if (dto.UpdateByMember && data.OwnerAccountId != dto.OwnerAccountId)
                throw new InvalidOperationException("非本人無法更新。");

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync(ct);
                try
                {
                    data.Status = (byte)dto.Status;
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

        public async Task<bool> UpdateAlbumCoverPathAsync(Guid albumId, string fileKey, Guid operatorId, CancellationToken ct)
        {
            var result = false;

            using var ctx = _factory.Create(ConnectionMode.Master);
            var db = ctx.AsDbContext<MyAlbumContext>();
            var strategy = db.Database.CreateExecutionStrategy();

            var album = await db.Albums
                .FirstOrDefaultAsync(m => m.AlbumId == albumId, ct);

            if (album == null)
            {
                throw new InvalidOperationException("相簿不存在。");
            }

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync(ct);
                try
                {
                    DateTime now = DateTime.UtcNow;

                    album.CoverPath = fileKey;
                    album.UpdatedBy = operatorId;

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
