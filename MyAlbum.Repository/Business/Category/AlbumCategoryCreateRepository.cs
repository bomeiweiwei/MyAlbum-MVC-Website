using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.Category;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.Category;
using MyAlbum.Models.Identity;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository.Business.Category
{
    public class AlbumCategoryCreateRepository : IAlbumCategoryCreateRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public AlbumCategoryCreateRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<Guid> CreateAlbumCategoryAsync(AlbumCategoryCreateDto dto, CancellationToken ct = default)
        {
            Guid result = Guid.Empty;

            using var ctx = _factory.Create(ConnectionMode.Master);
            var db = ctx.AsDbContext<MyAlbumContext>();
            var strategy = db.Database.CreateExecutionStrategy();


            var exists = await db.AlbumCategories.AsNoTracking()
                .AnyAsync(a => a.CategoryName == dto.CategoryName, ct);

            if (exists)
            {
                throw new InvalidOperationException("類別名稱已存在。");
            }

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync(ct);
                try
                {
                    DateTime now = DateTime.UtcNow;

                    var albumCategory = new Infrastructure.EF.Models.AlbumCategory
                    {
                        AlbumCategoryId = dto.AlbumCategoryId,
                        CategoryName = dto.CategoryName,
                        SortOrder = dto.SortOrder,
                        Status = (int)Status.Active,
                        CreatedAtUtc = now,
                        CreatedBy = dto.CreatedBy,
                        UpdatedAtUtc = now,
                        UpdatedBy = dto.CreatedBy
                    };
                    await db.AlbumCategories.AddAsync(albumCategory, ct);

                    await db.SaveChangesAsync(ct);

                    await tx.CommitAsync(ct);

                    result = albumCategory.AlbumCategoryId;
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
