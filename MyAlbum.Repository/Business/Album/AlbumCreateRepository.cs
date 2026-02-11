using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.Album;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.Album;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository.Business.Album
{
    public class AlbumCreateRepository : IAlbumCreateRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public AlbumCreateRepository(IAlbumDbContextFactory factory) => _factory = factory;
        public async Task<Guid> CreateAlbumAsync(AlbumCreateDto dto, CancellationToken ct = default)
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
                    var data = new Infrastructure.EF.Models.Album
                    {
                        AlbumId = dto.AlbumId,
                        AlbumCategoryId = dto.AlbumCategoryId,
                        OwnerAccountId = dto.OwnerAccountId,
                        Title = dto.Title,
                        Description = dto.Description,
                        CoverPath = dto.CoverPath,
                        ReleaseTimeUtc = dto.ReleaseTimeUtc,
                        TotalCommentNum = dto.TotalCommentNum,
                        Status = (byte)Status.Active,
                        CreatedAtUtc = dto.CreatedAtUtc,
                        CreatedBy = dto.CreatedBy,
                        UpdatedAtUtc = dto.CreatedAtUtc,
                        UpdatedBy = dto.CreatedBy
                    };
                    await db.Albums.AddAsync(data, ct);

                    await db.SaveChangesAsync(ct);

                    await tx.CommitAsync(ct);

                    result = data.AlbumId;
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
