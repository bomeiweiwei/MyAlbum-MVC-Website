using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.AlbumComment;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.AlbumComment;
using MyAlbum.Models.Base;
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

            if (result != null)
            {
                result.ReleaseTimeUtc = DateTime.SpecifyKind(result.ReleaseTimeUtc, DateTimeKind.Utc);
                result.CreatedAtUtc = DateTime.SpecifyKind(result.CreatedAtUtc, DateTimeKind.Utc);
                result.UpdatedAtUtc = DateTime.SpecifyKind(result.UpdatedAtUtc, DateTimeKind.Utc);
            }

            return result;
        }

        public async Task<ResponseBase<List<AlbumCommentDto>>> GetAlbumCommentListAsync(PageRequestBase<GetAlbumCommentListReq> req, CancellationToken ct = default)
        {
            var result = new ResponseBase<List<AlbumCommentDto>>();

            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<MyAlbumContext>();

            var query =
                from main in db.AlbumComments.AsNoTracking()
                join member in db.Members.AsNoTracking() on main.MemberId equals member.MemberId into memberGroup
                from member in memberGroup.DefaultIfEmpty()
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
                    UpdatedBy = main.UpdatedBy,
                    DisplayName = member.DisplayName
                };

            if (req.Data.AlbumCommentId.HasValue) { query = query.Where(x => x.AlbumCommentId == req.Data.AlbumCommentId.Value); }
            if (req.Data.AlbumPhotoId.HasValue) { query = query.Where(x => x.AlbumPhotoId == req.Data.AlbumPhotoId.Value); }
            if (req.Data.MemberId.HasValue) { query = query.Where(x => x.MemberId == req.Data.MemberId.Value); }

            if (!string.IsNullOrWhiteSpace(req.Data.Comment)) { query = query.Where(m => m.Comment.Contains(req.Data.Comment)); }

            if (req.Data.IsChanged.HasValue) { query = query.Where(x => x.IsChanged == req.Data.IsChanged.Value); }

            if (req.Data.Status.HasValue) { query = query.Where(x => x.Status == req.Data.Status.Value); }

            if (req.Data.ReleaseTimeUtc.HasValue) { query = query.Where(m => m.ReleaseTimeUtc == req.Data.ReleaseTimeUtc.Value); }

            if (req.Data.StartReleaseTimeUtc.HasValue) { query = query.Where(m => m.ReleaseTimeUtc >= req.Data.StartReleaseTimeUtc.Value); }
            if (req.Data.EndReleaseTimeUtc.HasValue) { query = query.Where(m => m.ReleaseTimeUtc < req.Data.EndReleaseTimeUtc.Value); }

            result.Count = await query.CountAsync(ct);
            result.Data = await query.Skip((req.pageIndex - 1) * req.pageSize).Take(req.pageSize).AsNoTracking().ToListAsync(ct);

            foreach (var item in result.Data)
            {
                item.ReleaseTimeUtc = DateTime.SpecifyKind(item.ReleaseTimeUtc, DateTimeKind.Utc);
                item.CreatedAtUtc = DateTime.SpecifyKind(item.CreatedAtUtc, DateTimeKind.Utc);
                item.UpdatedAtUtc = DateTime.SpecifyKind(item.UpdatedAtUtc, DateTimeKind.Utc);
            }

            return result;
        }
    }
}
