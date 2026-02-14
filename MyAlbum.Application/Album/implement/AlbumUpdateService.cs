using MyAlbum.Application.Member;
using MyAlbum.Application.Member.implement;
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
    public class AlbumUpdateService : BaseService, IAlbumUpdateService
    {
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IAlbumUpdateRepository _albumUpdateRepository;
        private readonly IFileUploadManagerService _fileUploadManagerService;
        public AlbumUpdateService(
            IAlbumDbContextFactory factory,
            ICurrentUserAccessor currentUser,
            IAlbumUpdateRepository albumUpdateRepository,
            IFileUploadManagerService fileUploadManagerService) : base(factory)
        {
            _albumUpdateRepository = albumUpdateRepository;
            _currentUser = currentUser;
            _fileUploadManagerService = fileUploadManagerService;
        }

        public async Task<bool> UpdateAlbumAsync(UpdateAlbumReq req, IReadOnlyList<UploadFileStream> files, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            var type = _currentUser.GetRequiredAccountType();

            // 上傳檔案並取得檔案位置
            Dictionary<string, string> dict = new Dictionary<string, string>();
            int fileCount = files.Count;
            if (fileCount > 0)
            {
                UploadModel uploadModel = new UploadModel()
                {
                    Id = req.AlbumId,
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

            AlbumUpdateDto dto = new AlbumUpdateDto
            {
                AlbumId = req.AlbumId,
                AlbumCategoryId = req.AlbumCategoryId,
                OwnerAccountId = req.OwnerAccountId,
                Title = req.Title,
                Description = req.Description,
                CoverPath = !string.IsNullOrWhiteSpace(coverPath) ? coverPath : null,
                Status = req.Status,
                UpdatedAtUtc = DateTime.UtcNow,
                UpdatedBy = operatorId
            };
            if (type == AccountType.Member)
            {
                // 覆蓋
                dto.OwnerAccountId = operatorId;
                dto.UpdateByMember = true;
            }
            else
            {
                dto.UpdateByMember = false;
            }

            var result = await _albumUpdateRepository.UpdateAlbumAsync(dto, ct);
            return result;
        }

        public async Task<bool> UpdateAlbumActiveAsync(UpdateAlbumActiveReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            var type = _currentUser.GetRequiredAccountType();
            
            AlbumUpdateActiveDto dto = new AlbumUpdateActiveDto
            {
                AlbumId = req.AlbumId,
                OwnerAccountId = req.OwnerAccountId,
                Status = req.Status,
                UpdatedAtUtc = DateTime.UtcNow,
                UpdateBy = operatorId,
            };
            if (type == AccountType.Member)
            {
                dto.OwnerAccountId = operatorId;
                dto.UpdateByMember = true;
            }
            else
            {
                dto.UpdateByMember = false;
            }

            return await _albumUpdateRepository.UpdateAlbumActiveAsync(dto, ct);
        }
    }
}
