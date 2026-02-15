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
    public sealed class AlbumCommentCreateRepository : IAlbumCommentCreateRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public AlbumCommentCreateRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<Guid> CreateAlbumCommentAsync(AlbumCommentCreateDto dto, CancellationToken ct = default)
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
                    /* 設定資料區塊 */
                    var data = new Infrastructure.EF.Models.AlbumComment
                    {
                        AlbumCommentId = dto.AlbumCommentId,
                        AlbumPhotoId = dto.AlbumPhotoId,
                        MemberId = dto.MemberId,
                        Comment = dto.Comment,
                        ReleaseTimeUtc = dto.ReleaseTimeUtc,
                        IsChanged = dto.IsChanged,

                        // DB 欄位型別是 byte（或 tinyint），系統內是 Enum Status
                        Status = (byte)dto.Status,

                        CreatedAtUtc = dto.CreatedAtUtc,
                        CreatedBy = dto.CreatedBy,
                        UpdatedAtUtc = dto.CreatedAtUtc,
                        UpdatedBy = dto.CreatedBy
                    };

                    await db.AlbumComments.AddAsync(data, ct);

                    await db.SaveChangesAsync(ct);

                    var photoAffected = await db.AlbumPhotos
                                        .Where(p => p.AlbumPhotoId == dto.AlbumPhotoId)
                                        .ExecuteUpdateAsync(s => s.SetProperty(p => p.CommentNum, p => p.CommentNum + 1), ct);

                    if (photoAffected != 1)
                        throw new InvalidOperationException("AlbumPhoto not found");

                    var albumId = await db.AlbumPhotos
                                                    .Where(p => p.AlbumPhotoId == dto.AlbumPhotoId)
                                                    .Select(p => p.AlbumId)
                                                    .SingleAsync(ct);

                    await db.Albums
                                .Where(a => a.AlbumId == albumId)
                                .ExecuteUpdateAsync(s => s.SetProperty(a => a.TotalCommentNum, a => a.TotalCommentNum + 1), ct);


                    await tx.CommitAsync(ct);

                    result = data.AlbumCommentId;
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
