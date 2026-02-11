using MyAlbum.Domain;
using MyAlbum.Domain.AlbumPhoto;
using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Shared.Idenyity;
using MyAlbum.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumPhoto.implement
{
    public class AlbumPhotoUpdateService:BaseService, IAlbumPhotoUpdateService
    {
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IAlbumPhotoUpdateRepository _albumPhotoUpdateRepository;
        public AlbumPhotoUpdateService(
            IAlbumDbContextFactory factory,
            ICurrentUserAccessor currentUser,
            IAlbumPhotoUpdateRepository albumPhotoUpdateRepository) : base(factory)
        {
            _albumPhotoUpdateRepository = albumPhotoUpdateRepository;
            _currentUser = currentUser;
        }

        public async Task<bool> UpdateAlbumPhotoAsync(UpdateAlbumPhotoReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            AlbumPhotoUpdateDto dto = new AlbumPhotoUpdateDto();
            dto.AlbumPhotoId = req.AlbumPhotoId;
            dto.AlbumId = req.AlbumId;
            dto.FilePath = req.FilePath;
            dto.OriginalFileName= req.OriginalFileName;
            dto.ContentType = req.ContentType;
            dto.FileSizeBytes = req.FileSizeBytes;
            dto.SortOrder = req.SortOrder;
            dto.Status = req.Status;
            dto.UpdatedBy = operatorId;
            return await _albumPhotoUpdateRepository.UpdateAlbumPhotoAsync(dto, ct);
        }

        public async Task<bool> UpdateAlbumPhotoActiveAsync(UpdateAlbumPhotoActiveReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            AlbumPhotoUpdateActiveDto dto = new AlbumPhotoUpdateActiveDto();
            dto.AlbumPhotoId = req.AlbumPhotoId;
            dto.Status = req.Status;
            dto.UpdatedBy = operatorId;
            return await _albumPhotoUpdateRepository.UpdateAlbumPhotoActiveAsync(dto, ct);
        }
    }
}
