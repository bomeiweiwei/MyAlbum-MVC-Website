using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.Category;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.Base;
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
                            cat.AlbumCategoryId == req.AlbumCategoryId
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

        public async Task<ResponseBase<List<AlbumCategoryDto>>> GetAlbumCategoryListAsync(PageRequestBase<GetAlbumCategoryReq> req, CancellationToken ct = default)
        {
            var result = new ResponseBase<List<AlbumCategoryDto>>();
            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<MyAlbumContext>();

            var query = from cat in db.AlbumCategories.AsNoTracking()
                        orderby cat.SortOrder
                        select new AlbumCategoryDto()
                        {
                            AlbumCategoryId = cat.AlbumCategoryId,
                            CategoryName = cat.CategoryName,
                            SortOrder = cat.SortOrder,
                            Status = (Status)cat.Status
                        };

            if (!string.IsNullOrEmpty(req.Data.CategoryName))
            {
                query = query.Where(x => x.CategoryName.Contains(req.Data.CategoryName));
            }
            if (req.Data.Status.HasValue)
            {
                query = query.Where(m => m.Status == req.Data.Status);
            }

            result.Count = await query.CountAsync();
            result.Data = await query.Skip((req.pageIndex - 1) * req.pageSize).Take(req.pageSize).AsNoTracking().ToListAsync(ct);

            return result;
        }
    }
}
