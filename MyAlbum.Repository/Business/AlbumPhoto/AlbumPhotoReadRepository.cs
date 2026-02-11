using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.AlbumPhoto;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.AlbumPhoto;
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
                select new AlbumPhotoDto
                {
                    AlbumPhotoId = main.AlbumPhotoId,
                    AlbumId = main.AlbumId,
                    FilePath = main.FilePath,
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
            return result;
        }

        public async Task<List<AlbumPhotoDto>> GetAlbumPhotoListAsync(GetAlbumPhotoReq req, CancellationToken ct = default)
        {
            var result = new List<AlbumPhotoDto>();

            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<MyAlbumContext>();

            var query =
                from main in db.AlbumPhotos.AsNoTracking()
                select new AlbumPhotoDto
                {
                    AlbumPhotoId = main.AlbumPhotoId,
                    AlbumId = main.AlbumId,
                    FilePath = main.FilePath,
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

            result = await query.ToListAsync(ct);
            return result;
        }
    }
}
