using MyAlbum.Application.Uploads;
using MyAlbum.Domain;
using MyAlbum.Domain.AlbumPhoto;
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
    public class AlbumPhotoUpdateService:BaseService, IAlbumPhotoUpdateService
    {
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IAlbumPhotoUpdateRepository _albumPhotoUpdateRepository;
        private readonly IFileUploadManagerService _fileUploadManagerService;
        public AlbumPhotoUpdateService(
            IAlbumDbContextFactory factory,
            ICurrentUserAccessor currentUser,
            IAlbumPhotoUpdateRepository albumPhotoUpdateRepository,
            IFileUploadManagerService fileUploadManagerService) : base(factory)
        {
            _albumPhotoUpdateRepository = albumPhotoUpdateRepository;
            _currentUser = currentUser;
            _fileUploadManagerService = fileUploadManagerService;
        }

        public async Task<bool> UpdateAlbumPhotoAsync(UpdateAlbumPhotoReq req, IReadOnlyList<UploadFileStream> files, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            var type = _currentUser.GetRequiredAccountType();

            Dictionary<string, string> dict = new Dictionary<string, string>();
            int fileCount = files.Count;
            if (fileCount > 0)
            {
                UploadModel uploadModel = new UploadModel()
                {
                    Id = req.AlbumPhotoId,
                    ColumnType = 1,
                    EntityUploadType = EntityUploadType.AlbumPhoto
                };
                dict = await _fileUploadManagerService.FileUpload(uploadModel, files, ct);
            }

            AlbumPhotoUpdateDto dto = new AlbumPhotoUpdateDto();
            dto.AlbumPhotoId = req.AlbumPhotoId;
            dto.AlbumId = req.AlbumId;
            dto.FilePath = null;
            dto.OriginalFileName = null;
            dto.ContentType = null;
            dto.FileSizeBytes = null;

            string path = string.Empty;
            if (dict.Count > 0)
            {
                var chkFile = files.First();
                if (dict.TryGetValue(chkFile.FileName, out var name))
                {
                    path = name;
                    dto.FilePath = path;
                    dto.OriginalFileName = chkFile.FileName;
                    dto.ContentType = chkFile.ContentType;
                    dto.FileSizeBytes = chkFile.Length;
                }
            }
            dto.SortOrder = req.SortOrder;
            dto.Status = req.Status;
            dto.UpdatedAtUtc = DateTime.UtcNow;
            dto.UpdatedBy = operatorId;
            if (type == AccountType.Member)
            {
                dto.OwnerAccountId = operatorId;
                dto.UpdateByMember = true;
            }
            else
            {
                dto.UpdateByMember = false;
            }

            return await _albumPhotoUpdateRepository.UpdateAlbumPhotoAsync(dto, ct);
        }

        public async Task<bool> UpdateAlbumPhotoActiveAsync(UpdateAlbumPhotoActiveReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            var type = _currentUser.GetRequiredAccountType();

            AlbumPhotoUpdateActiveDto dto = new AlbumPhotoUpdateActiveDto();
            dto.AlbumPhotoId = req.AlbumPhotoId;
            dto.Status = req.Status;
            dto.UpdatedAtUtc = DateTime.UtcNow;
            dto.UpdatedBy = operatorId;
            if (type == AccountType.Member)
            {
                dto.OwnerAccountId = operatorId;
                dto.UpdateByMember = true;
            }
            else
            {
                dto.UpdateByMember = false;
            }

            return await _albumPhotoUpdateRepository.UpdateAlbumPhotoActiveAsync(dto, ct);
        }
    }
}
