using MyAlbum.Application.Uploads;
using MyAlbum.Domain;
using MyAlbum.Domain.AlbumPhoto;
using MyAlbum.Domain.Category;
using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Models.UploadFiles;
using MyAlbum.Shared.Enums;
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
        private readonly IFileUploadManagerService _fileUploadManagerService;
        public AlbumPhotoCreateService(
            IAlbumDbContextFactory factory,
            ICurrentUserAccessor currentUser,
            IAlbumPhotoCreateRepository albumPhotoCreateRepository,
            IFileUploadManagerService fileUploadManagerService) : base(factory)
        {
            _albumPhotoCreateRepository = albumPhotoCreateRepository;
            _currentUser = currentUser;
            _fileUploadManagerService = fileUploadManagerService;
        }

        public async Task<Guid> CreateAlbumPhotoAsync(CreateAlbumPhotoReq req, IReadOnlyList<UploadFileStream> files, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();

            List<AlbumPhotoCreateDto> list = new List<AlbumPhotoCreateDto>();
            // 上傳檔案並取得檔案位置
            Dictionary<string, string> dict = new Dictionary<string, string>();
            int fileCount = files.Count;
            if (fileCount > 0)
            {
                UploadModel uploadModel = new UploadModel()
                {
                    Id = req.AlbumId,
                    ColumnType = 1,
                    EntityUploadType = EntityUploadType.AlbumPhoto
                };
                dict = await _fileUploadManagerService.FileUpload(uploadModel, files, ct);

                foreach (var file in files)
                {
                    string path = "";
                    if (dict.TryGetValue(file.FileName, out var name))
                    {
                        path = name;
                    }
                    AlbumPhotoCreateDto dto = new AlbumPhotoCreateDto()
                    {
                        AlbumId = req.AlbumId,
                        FilePath = path,
                        OriginalFileName = file.FileName,
                        ContentType = file.ContentType,
                        FileSizeBytes = file.Length,
                        SortOrder = 0,
                        CommentNum = 0,
                        CreatedBy = operatorId,
                        CreatedAtUtc = DateTime.UtcNow,
                    };
                    list.Add(dto);
                }
            }
            if (list.Count > 0)
                return await _albumPhotoCreateRepository.CreateAlbumPhotoAsync(list, ct);
            else
                return Guid.Empty;
        }
    }
}
