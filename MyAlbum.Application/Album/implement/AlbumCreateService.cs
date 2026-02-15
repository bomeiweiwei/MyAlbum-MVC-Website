using MyAlbum.Application.Uploads;
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
        private readonly IFileUploadManagerService _fileUploadManagerService;
        public AlbumCreateService(
            IAlbumDbContextFactory factory,
            ICurrentUserAccessor currentUser,
            IAlbumCreateRepository albumCreateRepository,
            IFileUploadManagerService fileUploadManagerService) : base(factory)
        {
            _albumCreateRepository = albumCreateRepository;
            _currentUser = currentUser;
            _fileUploadManagerService = fileUploadManagerService;
        }

        public async Task<Guid> CreateAlbumAsync(CreateAlbumReq req, IReadOnlyList<UploadFileStream> files, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();

            Guid albumId = Guid.NewGuid();

            // 上傳檔案並取得檔案位置
            Dictionary<string, string> dict = new Dictionary<string, string>();
            int fileCount = files.Count;
            if (fileCount > 0)
            {
                UploadModel uploadModel = new UploadModel()
                {
                    Id = albumId,
                    ColumnType = 1,
                    EntityUploadType = EntityUploadType.Album
                };
                dict = await _fileUploadManagerService.FileUpload(uploadModel, files, ct);
            }
            string coverPath = string.Empty;
            if (dict.Count > 0)
            {
                var chkFile = files.First();
                if (dict.TryGetValue(chkFile.FileName, out var name))
                {
                    coverPath = name;
                }
            }

            AlbumCreateDto dto = new AlbumCreateDto
            {
                AlbumId = albumId,
                AlbumCategoryId = req.AlbumCategoryId,
                OwnerAccountId = req.OwnerAccountId,
                Title = req.Title,
                Description = req.Description,
                CoverPath = !string.IsNullOrWhiteSpace(coverPath) ? coverPath : null,
                ReleaseTimeUtc = DateTime.UtcNow,
                TotalCommentNum = 0,
                CreatedBy = operatorId,
                CreatedAtUtc = DateTime.UtcNow,
            };

            var id = await _albumCreateRepository.CreateAlbumAsync(dto, ct);
            return id;
        }
    }
}
