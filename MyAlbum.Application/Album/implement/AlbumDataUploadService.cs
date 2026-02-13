using MyAlbum.Application.Uploads;
using MyAlbum.Domain;
using MyAlbum.Domain.Album;
using MyAlbum.Domain.Member;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.Idenyity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Album.implement
{
    public class AlbumDataUploadService : BaseService, IAlbumDataUploadService
    {
        private readonly IUploadPathService _paths;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IAlbumUpdateRepository _albumUpdateRepository;
        public AlbumDataUploadService(
           IAlbumDbContextFactory factory,
           IUploadPathService paths,
           ICurrentUserAccessor currentUser,
           IAlbumUpdateRepository albumUpdateRepository
           ) : base(factory)
        {
            _currentUser = currentUser;
            _paths = paths;
            _albumUpdateRepository = albumUpdateRepository;
        }

        public async Task<string> UploadCoverPathAsync(Guid albumId, Stream fileStream, string originalFileName, Mode mode, CancellationToken ct)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            var fileKey = _paths.BuildAlbumCoverPathFileKey(albumId, originalFileName);

            var physicalPath = _paths.ToPhysicalPath(fileKey);
            Directory.CreateDirectory(Path.GetDirectoryName(physicalPath)!);

            try
            {
                await using (var outStream = File.Create(physicalPath))
                {
                    await fileStream.CopyToAsync(outStream, ct);
                }
                await _albumUpdateRepository.UpdateAlbumCoverPathAsync(albumId, fileKey, operatorId, ct);
                return _paths.ToPublicUrl(fileKey);
            }
            catch
            {
                try { if (File.Exists(physicalPath)) File.Delete(physicalPath); } catch { /* ignore */ }
                throw;
            }
        }
    }
}
