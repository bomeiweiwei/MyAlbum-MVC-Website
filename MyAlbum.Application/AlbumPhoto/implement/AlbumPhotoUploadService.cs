using MyAlbum.Application.Album;
using MyAlbum.Application.Album.implement;
using MyAlbum.Application.Uploads;
using MyAlbum.Domain;
using MyAlbum.Models.UploadFiles;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumPhoto.implement
{
    public class AlbumPhotoUploadService : BaseService, IFileUploadService
    {
        public EntityUploadType entityUploadType => EntityUploadType.AlbumPhoto;
        private readonly IAlbumPhotosUploadService _albumPhotosUploadService;
        public AlbumPhotoUploadService(
            IAlbumDbContextFactory factory,
            IAlbumPhotosUploadService albumPhotosUploadService) : base(factory)
        {
            _albumPhotosUploadService = albumPhotosUploadService;
        }

        public async Task<Dictionary<string, string>> Upload(UploadModel model, IReadOnlyList<UploadFileStream> files, CancellationToken ct = default)
        {
            // filename, path
            Dictionary<string, string> dict = new Dictionary<string, string>();

            if (model.ColumnType == 1)
            {
                foreach (var file in files)
                {
                    string path = await _albumPhotosUploadService.UploadPathAsync(model, file.Stream, file.FileName, ct);
                    dict.Add(file.FileName, path);
                }
            }
            return dict;
        }
    }
}
