using MyAlbum.Application.Member;
using MyAlbum.Application.Member.implement;
using MyAlbum.Domain;
using MyAlbum.Domain.Album;
using MyAlbum.Domain.Category;
using MyAlbum.Models.Album;
using MyAlbum.Models.Category;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.Idenyity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Album.implement
{
    public class AlbumUpdateService : BaseService, IAlbumUpdateService
    {
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IAlbumUpdateRepository _albumUpdateRepository;
        private readonly IAlbumDataUploadService _albumDataUploadService;
        public AlbumUpdateService(
            IAlbumDbContextFactory factory,
            ICurrentUserAccessor currentUser,
            IAlbumUpdateRepository albumUpdateRepository,
            IAlbumDataUploadService albumDataUploadService) : base(factory)
        {
            _albumUpdateRepository = albumUpdateRepository;
            _currentUser = currentUser;
            _albumDataUploadService = albumDataUploadService;
        }

        public async Task<bool> UpdateAlbumAsync(UpdateAlbumReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            var type = _currentUser.GetRequiredAccountType();
            if (type == AccountType.Member)
                req.OwnerAccountId = operatorId;
            AlbumUpdateDto dto = new AlbumUpdateDto
            {
                AlbumId = req.AlbumId,
                AlbumCategoryId = req.AlbumCategoryId,
                OwnerAccountId = req.OwnerAccountId,
                Title = req.Title,
                Description = req.Description,
                Status = req.Status,
                UpdatedAtUtc = DateTime.UtcNow,
                UpdatedBy = operatorId
            };

            var result = await _albumUpdateRepository.UpdateAlbumAsync(dto, ct);

            // 上傳檔案並取得檔案位置
            if (result && req.FileBytes != null && !string.IsNullOrWhiteSpace(req.FileName))
            {
                await using var stream = new MemoryStream(req.FileBytes);
                var path = await _albumDataUploadService.UploadCoverPathAsync(req.AlbumId, stream, req.FileName, Mode.Update, ct);
            }
            return result;
        }

        public async Task<bool> UpdateAlbumActiveAsync(UpdateAlbumActiveReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            var type = _currentUser.GetRequiredAccountType();
            if (type == AccountType.Member)
                req.OwnerAccountId = operatorId;
            AlbumUpdateActiveDto dto = new AlbumUpdateActiveDto
            {
                AlbumId = req.AlbumId,
                OwnerAccountId = req.OwnerAccountId,
                Status = req.Status,
                UpdatedAtUtc = DateTime.UtcNow,
                UpdateBy = operatorId,
            };
            return await _albumUpdateRepository.UpdateAlbumActiveAsync(dto, ct);
        }
    }
}
