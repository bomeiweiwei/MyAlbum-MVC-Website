using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.AlbumPhoto;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Infrastructure.EF.Models;
using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Models.Base;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository.Business.AlbumPhoto
{
    public sealed class AlbumPhotoReadRepository : IAlbumPhotoReadRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public AlbumPhotoReadRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<AlbumPhotoDto?> GetAlbumPhotoAsync(GetAlbumPhotoReq req, CancellationToken ct = default)
        {
            var result = new AlbumPhotoDto();

            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<MyAlbumContext>();

            var query =
                from main in db.AlbumPhotos.AsNoTracking()
                join album in db.Albums.AsNoTracking() on main.AlbumId equals album.AlbumId into photoGroup
                from album in photoGroup.DefaultIfEmpty()
                join member in db.Members.AsNoTracking() on album.OwnerAccountId equals member.AccountId into memberGroup
                from member in memberGroup.DefaultIfEmpty()
                select new AlbumPhotoDto
                {
                    OwnerName = member.DisplayName,
                    OwnerAccountId = album.OwnerAccountId,
                    Title = album.Title,
                    AlbumPhotoId = main.AlbumPhotoId,
                    AlbumId = main.AlbumId,
                    PublicPathUrl = main.FilePath,
                    OriginalFileName = main.OriginalFileName,
                    ContentType = main.ContentType,
                    FileSizeBytes = main.FileSizeBytes,
                    SortOrder = main.SortOrder,
                    CommentNum = main.CommentNum,
                    Status = (Status)main.Status,
                    CreatedAtUtc = main.CreatedAtUtc,
                    UpdatedAtUtc = main.UpdatedAtUtc,
                    CreatedBy = main.CreatedBy,
                    UpdatedBy = main.UpdatedBy
                };

            if (req.AlbumPhotoId.HasValue) 
            { 
                query = query.Where(x => x.AlbumPhotoId == req.AlbumPhotoId.Value); 
            }
            if (req.AlbumId.HasValue) 
            { 
                query = query.Where(x => x.AlbumId == req.AlbumId.Value); 
            }

            //if (!string.IsNullOrWhiteSpace(req.FilePath)) { query = query.Where(m => m.FilePath.Contains(req.FilePath)); }
            //if (!string.IsNullOrWhiteSpace(req.OriginalFileName)) { query = query.Where(m => (m.OriginalFileName ?? "").Contains(req.OriginalFileName)); }
            //if (!string.IsNullOrWhiteSpace(req.ContentType)) { query = query.Where(m => (m.ContentType ?? "").Contains(req.ContentType)); }

            //if (req.FileSizeBytes.HasValue) { query = query.Where(x => x.FileSizeBytes == req.FileSizeBytes.Value); }
            //if (req.SortOrder.HasValue) { query = query.Where(x => x.SortOrder == req.SortOrder.Value); }
            //if (req.CommentNum.HasValue) { query = query.Where(x => x.CommentNum == req.CommentNum.Value); }

            if (req.Status.HasValue) 
            {
                query = query.Where(x => x.Status == req.Status.Value);
            }

            result = await query.FirstOrDefaultAsync(ct);

            if (result != null)
            {
                result.CreatedAtUtc = DateTime.SpecifyKind(result.CreatedAtUtc, DateTimeKind.Utc);
                result.UpdatedAtUtc = DateTime.SpecifyKind(result.UpdatedAtUtc, DateTimeKind.Utc);
            }

            return result;
        }

        public async Task<ResponseBase<List<AlbumPhotoDto>>> GetAlbumPhotoListAsync(PageRequestBase<GetAlbumPhotoReq> req, CancellationToken ct = default)
        {
            var result = new ResponseBase<List<AlbumPhotoDto>>();

            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<MyAlbumContext>();

            var query =
                from main in db.AlbumPhotos.AsNoTracking()
                join album in db.Albums.AsNoTracking() on main.AlbumId equals album.AlbumId into photoGroup
                from album in photoGroup.DefaultIfEmpty()
                join member in db.Members.AsNoTracking() on album.OwnerAccountId equals member.AccountId into memberGroup
                from member in memberGroup.DefaultIfEmpty()
                orderby main.AlbumId, main.SortOrder, main.CreatedAtUtc descending
                select new AlbumPhotoDto
                {
                    OwnerName = member.DisplayName,
                    OwnerAccountId = album.OwnerAccountId,
                    Title = album.Title,
                    AlbumPhotoId = main.AlbumPhotoId,
                    AlbumId = main.AlbumId,
                    PublicPathUrl = main.FilePath,
                    OriginalFileName = main.OriginalFileName,
                    ContentType = main.ContentType,
                    FileSizeBytes = main.FileSizeBytes,
                    SortOrder = main.SortOrder,
                    CommentNum = main.CommentNum,
                    Status = (Status)main.Status,
                    CreatedAtUtc = main.CreatedAtUtc,
                    UpdatedAtUtc = main.UpdatedAtUtc,
                    CreatedBy = main.CreatedBy,
                    UpdatedBy = main.UpdatedBy
                };

            if (req.Data.AlbumPhotoId.HasValue) 
            {
                query = query.Where(x => x.AlbumPhotoId == req.Data.AlbumPhotoId.Value);
            }
            if (req.Data.AlbumId.HasValue) 
            { 
                query = query.Where(x => x.AlbumId == req.Data.AlbumId.Value);
            }
            if (req.Data.OwnerAccountId.HasValue)
            {
                query = query.Where(x => x.OwnerAccountId == req.Data.OwnerAccountId);
            }
            //if (!string.IsNullOrWhiteSpace(req.FilePath)) { query = query.Where(m => m.FilePath.Contains(req.FilePath)); }
            //if (!string.IsNullOrWhiteSpace(req.OriginalFileName)) { query = query.Where(m => (m.OriginalFileName ?? "").Contains(req.OriginalFileName)); }
            //if (!string.IsNullOrWhiteSpace(req.ContentType)) { query = query.Where(m => (m.ContentType ?? "").Contains(req.ContentType)); }

            //if (req.FileSizeBytes.HasValue) { query = query.Where(x => x.FileSizeBytes == req.FileSizeBytes.Value); }
            //if (req.SortOrder.HasValue) { query = query.Where(x => x.SortOrder == req.SortOrder.Value); }
            //if (req.CommentNum.HasValue) { query = query.Where(x => x.CommentNum == req.CommentNum.Value); }

            if (req.Data.Status.HasValue) 
            { 
                query = query.Where(x => x.Status == req.Data.Status.Value); 
            }

            result.Count = await query.CountAsync(ct);
            result.Data = await query.Skip((req.pageIndex - 1) * req.pageSize).Take(req.pageSize).AsNoTracking().ToListAsync(ct);

            foreach (var item in result.Data)
            {
                item.CreatedAtUtc = DateTime.SpecifyKind(item.CreatedAtUtc, DateTimeKind.Utc);
                item.UpdatedAtUtc = DateTime.SpecifyKind(item.UpdatedAtUtc, DateTimeKind.Utc);
            }

            return result;
        }
    }
}
