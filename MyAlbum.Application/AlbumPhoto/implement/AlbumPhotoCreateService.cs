using MyAlbum.Domain;
using MyAlbum.Domain.AlbumPhoto;
using MyAlbum.Domain.Category;
using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.Idenyity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumPhoto.implement
{
    public class AlbumPhotoCreateService : BaseService, IAlbumPhotoCreateService
    {
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IAlbumPhotoCreateRepository _albumPhotoCreateRepository;
        public AlbumPhotoCreateService(
            IAlbumDbContextFactory factory,
            ICurrentUserAccessor currentUser,
            IAlbumPhotoCreateRepository albumPhotoCreateRepository) : base(factory)
        {
            _albumPhotoCreateRepository = albumPhotoCreateRepository;
            _currentUser = currentUser;
        }

        public async Task<Guid> CreateAlbumPhotoAsync(CreateAlbumPhotoReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            AlbumPhotoCreateDto dto = new AlbumPhotoCreateDto()
            {
                AlbumId = req.AlbumId,
                FilePath = req.FilePath,
                OriginalFileName = req.OriginalFileName,
                ContentType = req.ContentType,
                FileSizeBytes = req.FileSizeBytes,
                SortOrder = req.SortOrder,
                CommentNum = 0,
                CreatedBy = operatorId
            };
            return await _albumPhotoCreateRepository.CreateAlbumPhotoAsync(dto, ct);
        }
    }
}
