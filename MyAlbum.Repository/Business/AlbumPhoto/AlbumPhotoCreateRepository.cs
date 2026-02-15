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
    public sealed class AlbumPhotoCreateRepository : IAlbumPhotoCreateRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public AlbumPhotoCreateRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<Guid> CreateAlbumPhotoAsync(List<AlbumPhotoCreateDto> list, CancellationToken ct = default)
        {
            Guid result = Guid.Empty;

            using var ctx = _factory.Create(ConnectionMode.Master);
            var db = ctx.AsDbContext<MyAlbumContext>();
            var strategy = db.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync(ct);
                try
                {
                    foreach (var item in list)
                    {
                        var data = new Infrastructure.EF.Models.AlbumPhoto
                        {
                            AlbumPhotoId = item.AlbumPhotoId,
                            AlbumId = item.AlbumId,
                            FilePath = item.FilePath,
                            OriginalFileName = item.OriginalFileName,
                            ContentType = item.ContentType,
                            FileSizeBytes = item.FileSizeBytes,
                            SortOrder = item.SortOrder,
                            CommentNum = item.CommentNum,
                            Status = (byte)item.Status,
                            CreatedAtUtc = item.CreatedAtUtc,
                            CreatedBy = item.CreatedBy,
                            UpdatedAtUtc = item.CreatedAtUtc,
                            UpdatedBy = item.CreatedBy
                        };

                        await db.AlbumPhotos.AddAsync(data, ct);
                    }
                    await db.SaveChangesAsync(ct);

                    await tx.CommitAsync(ct);

                    result = list.First().AlbumPhotoId;
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
