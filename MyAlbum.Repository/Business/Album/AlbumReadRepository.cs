using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.Album;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.Album;
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
                        select new AlbumDto()
                        {
                            AlbumId = main.AlbumId,
                            AlbumCategoryId = main.AlbumCategoryId,
                            OwnerAccountId = main.OwnerAccountId,
                            Title = main.Title,
                            Description = main.Description,
                            CoverPath = main.CoverPath,
                            ReleaseTimeUtc = main.ReleaseTimeUtc,
                            TotalCommentNum = main.TotalCommentNum,
                            Status = (Status)main.Status,
                            CreatedAtUtc = main.CreatedAtUtc,
                            CreatedBy = main.CreatedBy,
                            UpdatedAtUtc = main.UpdatedAtUtc,
                            UpdatedBy = main.UpdatedBy,
                        };
            if (req.AlbumId.HasValue)
            {
                query = query.Where(x => x.AlbumId == req.AlbumId);
            }
            if (req.AlbumCategoryId.HasValue)
            {
                query = query.Where(x => x.AlbumCategoryId == req.AlbumCategoryId);
            }
            if (req.OwnerAccountId.HasValue)
            {
                query = query.Where(x => x.OwnerAccountId == req.OwnerAccountId);
            }
            if (!string.IsNullOrWhiteSpace(req.Title))
            {
                query = query.Where(m => m.Title.Contains(req.Title));
            }
            if (req.ReleaseTimeUtc.HasValue)
            {
                query = query.Where(m => m.ReleaseTimeUtc == req.ReleaseTimeUtc);
            }
            if (req.StartReleaseTimeUtc.HasValue)
            {
                query = query.Where(m => m.ReleaseTimeUtc >= req.StartReleaseTimeUtc);
            }
            if (req.EndReleaseTimeUtc.HasValue)
            {
                query = query.Where(m => m.ReleaseTimeUtc <= req.EndReleaseTimeUtc);
            }
            if (req.Status.HasValue)
            {
                query = query.Where(m => m.Status == req.Status);
            }

            result = await query.FirstOrDefaultAsync(ct);

            return result;
        }

        public async Task<List<AlbumDto>> GetAlbumListAsync(GetAlbumReq req, CancellationToken ct = default)
        {
            var result = new List<AlbumDto>();
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
                            CoverPath = main.CoverPath,
                            ReleaseTimeUtc = main.ReleaseTimeUtc,
                            TotalCommentNum = main.TotalCommentNum,
                            Status = (Status)main.Status,
                            CreatedAtUtc = main.CreatedAtUtc,
                            CreatedBy = main.CreatedBy,
                            UpdatedAtUtc = main.UpdatedAtUtc,
                            UpdatedBy = main.UpdatedBy,
                        };
            if (req.AlbumId.HasValue)
            {
                query = query.Where(x => x.AlbumId == req.AlbumId);
            }
            if (req.AlbumCategoryId.HasValue)
            {
                query = query.Where(x => x.AlbumCategoryId == req.AlbumCategoryId);
            }
            if (req.OwnerAccountId.HasValue)
            {
                query = query.Where(x => x.OwnerAccountId == req.OwnerAccountId);
            }
            if (!string.IsNullOrWhiteSpace(req.Title))
            {
                query = query.Where(m => m.Title.Contains(req.Title));
            }
            if (req.StartReleaseTimeUtc.HasValue)
            {
                query = query.Where(m => m.ReleaseTimeUtc >= req.StartReleaseTimeUtc);
            }
            if (req.EndReleaseTimeUtc.HasValue)
            {
                query = query.Where(m => m.ReleaseTimeUtc <= req.EndReleaseTimeUtc);
            }
            if (req.Status.HasValue)
            {
                query = query.Where(m => m.Status == req.Status);
            }

            result = await query.ToListAsync(ct);

            return result;
        }
    }
}
