using MyAlbum.Domain;
using MyAlbum.Domain.Album;
using MyAlbum.Domain.Category;
using MyAlbum.Models.Album;
using MyAlbum.Models.Category;
using MyAlbum.Models.UploadFiles;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.Idenyity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Album.implement
{
    public class AlbumCreateService : BaseService, IAlbumCreateService
    {
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IAlbumCreateRepository _albumCreateRepository;
        private readonly IAlbumDataUploadService _albumDataUploadService;
        public AlbumCreateService(
            IAlbumDbContextFactory factory,
            ICurrentUserAccessor currentUser,
            IAlbumCreateRepository albumCreateRepository,
            IAlbumDataUploadService albumDataUploadService) : base(factory)
        {
            _albumCreateRepository = albumCreateRepository;
            _currentUser = currentUser;
            _albumDataUploadService = albumDataUploadService;
        }

        public async Task<Guid> CreateAlbumAsync(CreateAlbumReq req, IReadOnlyList<UploadFileStream> files, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();

            AlbumCreateDto dto = new AlbumCreateDto
            {
                AlbumId = Guid.NewGuid(),
                AlbumCategoryId = req.AlbumCategoryId,
                OwnerAccountId = req.OwnerAccountId,
                Title = req.Title,
                Description = req.Description,
                CoverPath = "",
                ReleaseTimeUtc = DateTime.UtcNow,
                TotalCommentNum = 0,
                CreatedBy = operatorId,
                CreatedAtUtc = DateTime.UtcNow,
            };

            var id = await _albumCreateRepository.CreateAlbumAsync(dto, ct);

            // 上傳檔案並取得檔案位置
            var f = files.First();
            var coverPath = await _albumDataUploadService.UploadCoverPathAsync(id, f.Stream, f.FileName, Mode.Create, ct);

            return id;
        }
    }
}
