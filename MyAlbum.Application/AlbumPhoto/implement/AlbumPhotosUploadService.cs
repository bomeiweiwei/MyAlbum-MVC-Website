using MyAlbum.Application.Album;
using MyAlbum.Application.Uploads;
using MyAlbum.Domain;
using MyAlbum.Models.UploadFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumPhoto.implement
{
    public class AlbumPhotosUploadService : BaseService, IAlbumPhotosUploadService
    {
        private readonly IUploadPathService _paths;
        public AlbumPhotosUploadService(
           IAlbumDbContextFactory factory,
           IUploadPathService paths
           ) : base(factory)
        {
            _paths = paths;
        }

        public async Task<string> UploadPathAsync(UploadModel model, Stream fileStream, string originalFileName, CancellationToken ct)
        {
            var fileKey = _paths.BuildAlbumPhotoPathFileKey(model.Id, originalFileName);

            var physicalPath = _paths.ToPhysicalPath(fileKey);
            Directory.CreateDirectory(Path.GetDirectoryName(physicalPath)!);

            try
            {
                await using (var outStream = File.Create(physicalPath))
                {
                    await fileStream.CopyToAsync(outStream, ct);
                }

                return fileKey;
            }
            catch
            {
                try { if (File.Exists(physicalPath)) File.Delete(physicalPath); } catch { /* ignore */ }
                throw;
            }
        }
    }
}
