using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.AlbumPhoto;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository.Business.AlbumPhoto
{
    public sealed class AlbumPhotoUpdateRepository : IAlbumPhotoUpdateRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public AlbumPhotoUpdateRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<bool> UpdateAlbumPhotoAsync(AlbumPhotoUpdateDto dto, CancellationToken ct = default)
        {
            bool result = false;

            using var ctx = _factory.Create(ConnectionMode.Master);
            var db = ctx.AsDbContext<MyAlbumContext>();
            var strategy = db.Database.CreateExecutionStrategy();

            var data = await db.AlbumPhotos
                .FirstOrDefaultAsync(m => m.AlbumPhotoId == dto.AlbumPhotoId && m.AlbumId == dto.AlbumId, ct);

            if (data == null)
                throw new InvalidOperationException("找不到指定的資料。");

            var checkOwnerAccountId = await (from photo in db.AlbumPhotos.AsNoTracking()
                                             join album in db.Albums.AsNoTracking() on photo.AlbumId equals album.AlbumId
                                             where photo.AlbumPhotoId == data.AlbumPhotoId && photo.AlbumId == data.AlbumId
                                             select album.OwnerAccountId).FirstOrDefaultAsync(ct);
            if (dto.UpdateByMember && checkOwnerAccountId != dto.OwnerAccountId)
                throw new InvalidOperationException("非本人無法更新。");

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync(ct);
                try
                {
                    data.FilePath = dto.FilePath ?? data.FilePath;
                    data.OriginalFileName = dto.OriginalFileName ?? data.OriginalFileName;
                    data.ContentType = dto.ContentType ?? data.ContentType;
                    data.FileSizeBytes = dto.FileSizeBytes ?? data.FileSizeBytes;

                    data.SortOrder = dto.SortOrder;
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

        public async Task<bool> UpdateAlbumPhotoActiveAsync(AlbumPhotoUpdateActiveDto dto, CancellationToken ct = default)
        {
            bool result = false;

            using var ctx = _factory.Create(ConnectionMode.Master);
            var db = ctx.AsDbContext<MyAlbumContext>();
            var strategy = db.Database.CreateExecutionStrategy();

            var data = await db.AlbumPhotos.FirstOrDefaultAsync(m => m.AlbumPhotoId == dto.AlbumPhotoId, ct);

            if (data == null)
                throw new InvalidOperationException("找不到指定的資料。");

            var checkOwnerAccountId = await (from photo in db.AlbumPhotos.AsNoTracking()
                                             join album in db.Albums.AsNoTracking() on photo.AlbumId equals album.AlbumId
                                             where photo.AlbumPhotoId == data.AlbumPhotoId
                                             select album.OwnerAccountId).FirstOrDefaultAsync(ct);
            if (dto.UpdateByMember && checkOwnerAccountId != dto.OwnerAccountId)
                throw new InvalidOperationException("非本人無法更新。");

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync(ct);
                try
                {
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
