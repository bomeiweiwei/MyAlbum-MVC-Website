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
    public sealed class AlbumCommentReadRepository : IAlbumCommentReadRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public AlbumCommentReadRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<AlbumCommentDto?> GetAlbumCommentAsync(GetAlbumCommentReq req, CancellationToken ct = default)
        {
            var result = new AlbumCommentDto();

            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<MyAlbumContext>();

            var query =
                from main in db.AlbumComments.AsNoTracking()
                select new AlbumCommentDto
                {
                    AlbumCommentId = main.AlbumCommentId,
                    AlbumPhotoId = main.AlbumPhotoId,
                    MemberId = main.MemberId,
                    Comment = main.Comment,
                    ReleaseTimeUtc = main.ReleaseTimeUtc,
                    IsChanged = main.IsChanged,
                    Status = (Status)main.Status,
                    CreatedAtUtc = main.CreatedAtUtc,
                    UpdatedAtUtc = main.UpdatedAtUtc,
                    CreatedBy = main.CreatedBy,
                    UpdatedBy = main.UpdatedBy
                };

            if (req.AlbumCommentId.HasValue) { query = query.Where(x => x.AlbumCommentId == req.AlbumCommentId.Value); }
            if (req.AlbumPhotoId.HasValue) { query = query.Where(x => x.AlbumPhotoId == req.AlbumPhotoId.Value); }
            if (req.MemberId.HasValue) { query = query.Where(x => x.MemberId == req.MemberId.Value); }

            if (!string.IsNullOrWhiteSpace(req.Comment)) { query = query.Where(m => m.Comment.Contains(req.Comment)); }

            if (req.IsChanged.HasValue) { query = query.Where(x => x.IsChanged == req.IsChanged.Value); }

            if (req.Status.HasValue) { query = query.Where(x => x.Status == req.Status.Value); }

            if (req.ReleaseTimeUtc.HasValue) { query = query.Where(m => m.ReleaseTimeUtc == req.ReleaseTimeUtc.Value); }
            if (req.StartReleaseTimeUtc.HasValue) { query = query.Where(m => m.ReleaseTimeUtc >= req.StartReleaseTimeUtc.Value); }
            if (req.EndReleaseTimeUtc.HasValue) { query = query.Where(m => m.ReleaseTimeUtc <= req.EndReleaseTimeUtc.Value); }

            result = await query.FirstOrDefaultAsync(ct);
            return result;
        }

        public async Task<List<AlbumCommentDto>> GetAlbumCommentListAsync(GetAlbumCommentReq req, CancellationToken ct = default)
        {
            var result = new List<AlbumCommentDto>();

            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<MyAlbumContext>();

            var query =
                from main in db.AlbumComments.AsNoTracking()
                select new AlbumCommentDto
                {
                    AlbumCommentId = main.AlbumCommentId,
                    AlbumPhotoId = main.AlbumPhotoId,
                    MemberId = main.MemberId,
                    Comment = main.Comment,
                    ReleaseTimeUtc = main.ReleaseTimeUtc,
                    IsChanged = main.IsChanged,
                    Status = (Status)main.Status,
                    CreatedAtUtc = main.CreatedAtUtc,
                    UpdatedAtUtc = main.UpdatedAtUtc,
                    CreatedBy = main.CreatedBy,
                    UpdatedBy = main.UpdatedBy
                };

            if (req.AlbumCommentId.HasValue) { query = query.Where(x => x.AlbumCommentId == req.AlbumCommentId.Value); }
            if (req.AlbumPhotoId.HasValue) { query = query.Where(x => x.AlbumPhotoId == req.AlbumPhotoId.Value); }
            if (req.MemberId.HasValue) { query = query.Where(x => x.MemberId == req.MemberId.Value); }

            if (!string.IsNullOrWhiteSpace(req.Comment)) { query = query.Where(m => m.Comment.Contains(req.Comment)); }

            if (req.IsChanged.HasValue) { query = query.Where(x => x.IsChanged == req.IsChanged.Value); }

            if (req.Status.HasValue) { query = query.Where(x => x.Status == req.Status.Value); }

            if (req.ReleaseTimeUtc.HasValue) { query = query.Where(m => m.ReleaseTimeUtc == req.ReleaseTimeUtc.Value); }
            if (req.StartReleaseTimeUtc.HasValue) { query = query.Where(m => m.ReleaseTimeUtc >= req.StartReleaseTimeUtc.Value); }
            if (req.EndReleaseTimeUtc.HasValue) { query = query.Where(m => m.ReleaseTimeUtc <= req.EndReleaseTimeUtc.Value); }

            result = await query.ToListAsync(ct);
            return result;
        }
    }
}
