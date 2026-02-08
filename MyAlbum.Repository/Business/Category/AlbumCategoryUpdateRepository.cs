using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.Category;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.Category;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository.Business.Category
{
    public class AlbumCategoryUpdateRepository : IAlbumCategoryUpdateRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public AlbumCategoryUpdateRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<bool> UpdateAlbumCategoryAsync(AlbumCategoryUpdateDto dto, CancellationToken ct = default)
        {
            bool result = false;

            using var ctx = _factory.Create(ConnectionMode.Master);
            var db = ctx.AsDbContext<MyAlbumContext>();
            var strategy = db.Database.CreateExecutionStrategy();

            var data = await db.AlbumCategories.FirstOrDefaultAsync(m => m.AlbumCategoryId == dto.AlbumCategoryId);
            if (data == null)
                throw new InvalidOperationException("找不到指定的類別。");

            var exists = await db.AlbumCategories.Where(m => m.AlbumCategoryId != data.AlbumCategoryId && m.CategoryName == dto.CategoryName)
                .AnyAsync(a => a.CategoryName == dto.CategoryName, ct);

            if (exists)
                throw new InvalidOperationException("類別名稱已存在。");

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync(ct);
                try
                {
                    DateTime now = DateTime.UtcNow;

                    data.CategoryName = dto.CategoryName;
                    data.SortOrder = dto.SortOrder;
                    data.Status = (byte)dto.Status;

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

        public async Task<bool> UpdateAlbumCategoryActiveAsync(AlbumCategoryUpdateActiveDto dto, CancellationToken ct = default)
        {
            bool result = false;

            using var ctx = _factory.Create(ConnectionMode.Master);
            var db = ctx.AsDbContext<MyAlbumContext>();
            var strategy = db.Database.CreateExecutionStrategy();

            var data = await db.AlbumCategories.FirstOrDefaultAsync(m => m.AlbumCategoryId == dto.AlbumCategoryId);
            if (data == null)
                throw new InvalidOperationException("找不到指定的類別。");

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync(ct);
                try
                {
                    DateTime now = DateTime.UtcNow;

                    data.Status = (byte)dto.Status;
                    data.UpdatedBy = dto.UpdatedBy;

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
