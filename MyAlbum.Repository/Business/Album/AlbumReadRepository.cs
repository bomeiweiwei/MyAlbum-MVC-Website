using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.Album;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.Album;
using MyAlbum.Models.Base;
using MyAlbum.Models.Category;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository.Business.Album
{
    public class AlbumReadRepository : IAlbumReadRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public AlbumReadRepository(IAlbumDbContextFactory factory) => _factory = factory;
        public async Task<AlbumDto?> GetAlbumAsync(GetAlbumReq req, CancellationToken ct = default)
        {
            var result = new AlbumDto();
            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<MyAlbumContext>();

            var query = from main in db.Albums.AsNoTracking()
                        where
                            main.AlbumId == req.AlbumId
                        select new AlbumDto()
                        {
                            AlbumId = main.AlbumId,
                            AlbumCategoryId = main.AlbumCategoryId,
                            OwnerAccountId = main.OwnerAccountId,
                            Title = main.Title,
                            Description = main.Description,
                            PublicCoverUrl = main.CoverPath ?? string.Empty,
                            ReleaseTimeUtc = main.ReleaseTimeUtc,
                            TotalCommentNum = main.TotalCommentNum,
                            Status = (Status)main.Status,
                            CreatedAtUtc = main.CreatedAtUtc,
                            CreatedBy = main.CreatedBy,
                            UpdatedAtUtc = main.UpdatedAtUtc,
                            UpdatedBy = main.UpdatedBy,
                        };
            if (req.AlbumCategoryId.HasValue)
            {
                query = query.Where(x => x.AlbumCategoryId == req.AlbumCategoryId);
            }
            if (req.OwnerAccountId.HasValue)
            {
                query = query.Where(x => x.OwnerAccountId == req.OwnerAccountId);
            }
            if (req.Status.HasValue)
            {
                query = query.Where(m => m.Status == req.Status);
            }

            result = await query.FirstOrDefaultAsync(ct);

            return result;
        }

        public async Task<ResponseBase<List<AlbumDto>>> GetAlbumListAsync(PageRequestBase<GetAlbumListReq> req, CancellationToken ct = default)
        {
            var result = new ResponseBase<List<AlbumDto>>();
            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<MyAlbumContext>();

            var query = from main in db.Albums.AsNoTracking()
                        select new AlbumDto()
                        {
                            AlbumId = main.AlbumId,
                            AlbumCategoryId = main.AlbumCategoryId,
                            OwnerAccountId = main.OwnerAccountId,
                            Title = main.Title,
                            Description = main.Description,
                            PublicCoverUrl = main.CoverPath ?? string.Empty,
                            ReleaseTimeUtc = main.ReleaseTimeUtc,
                            TotalCommentNum = main.TotalCommentNum,
                            Status = (Status)main.Status,
                            CreatedAtUtc = main.CreatedAtUtc,
                            CreatedBy = main.CreatedBy,
                            UpdatedAtUtc = main.UpdatedAtUtc,
                            UpdatedBy = main.UpdatedBy,
                        };
            if (req.Data.AlbumCategoryId.HasValue)
            {
                query = query.Where(x => x.AlbumCategoryId == req.Data.AlbumCategoryId);
            }
            if (req.Data.OwnerAccountId.HasValue)
            {
                query = query.Where(x => x.OwnerAccountId == req.Data.OwnerAccountId);
            }
            if (!string.IsNullOrWhiteSpace(req.Data.Title))
            {
                query = query.Where(m => m.Title.Contains(req.Data.Title));
            }
            if (req.Data.StartReleaseTimeUtc.HasValue)
            {
                query = query.Where(m => m.ReleaseTimeUtc >= req.Data.StartReleaseTimeUtc);
            }
            if (req.Data.EndReleaseTimeUtc.HasValue)
            {
                query = query.Where(m => m.ReleaseTimeUtc <= req.Data.EndReleaseTimeUtc);
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
