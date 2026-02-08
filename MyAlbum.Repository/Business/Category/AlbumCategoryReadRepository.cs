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
    public class AlbumCategoryReadRepository : IAlbumCategoryReadRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public AlbumCategoryReadRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<AlbumCategoryDto?> GetAlbumCategoryAsync(GetAlbumCategoryReq req, CancellationToken ct = default)
        {
            var result = new AlbumCategoryDto();
            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<MyAlbumContext>();

            var query = from cat in db.AlbumCategories.AsNoTracking()
                        where
                            cat.AlbumCategoryId == req.AlbumCategoryId &&
                            cat.Status == (int)Status.Active
                        select new AlbumCategoryDto()
                        {
                            AlbumCategoryId = cat.AlbumCategoryId,
                            CategoryName = cat.CategoryName,
                            SortOrder = cat.SortOrder,
                            Status = (Status)cat.Status
                        };
            if (!string.IsNullOrEmpty(req.CategoryName))
            {
                query = query.Where(x => x.CategoryName == req.CategoryName);
            }

            result = await query.FirstOrDefaultAsync(ct);

            return result;
        }

        public async Task<List<AlbumCategoryDto>> GetAlbumCategoryListAsync(GetAlbumCategoryReq req, CancellationToken ct = default)
        {
            var result = new List<AlbumCategoryDto>();
            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<MyAlbumContext>();

            var query = from cat in db.AlbumCategories.AsNoTracking()
                        where
                            cat.Status == (int)Status.Active
                        select new AlbumCategoryDto()
                        {
                            AlbumCategoryId = cat.AlbumCategoryId,
                            CategoryName = cat.CategoryName,
                            SortOrder = cat.SortOrder,
                            Status = (Status)cat.Status
                        };

            if (!string.IsNullOrEmpty(req.CategoryName))
            {
                query = query.Where(x => x.CategoryName == req.CategoryName);
            }

            result = await query.ToListAsync(ct);

            return result;
        }
    }
}
