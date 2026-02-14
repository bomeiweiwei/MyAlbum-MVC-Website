using MyAlbum.Application.Album;
using MyAlbum.Application.Uploads;
using MyAlbum.Domain;
using MyAlbum.Models.UploadFiles;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Album.implement
{
    public class AlbumUploadService : BaseService, IFileUploadService
    {
        public EntityUploadType entityUploadType => EntityUploadType.Album;
        private readonly IAlbumCoverUploadService _albumCoverUploadService;
        public AlbumUploadService(
            IAlbumDbContextFactory factory,
            IAlbumCoverUploadService albumCoverUploadService) :base(factory)
        {
            _albumCoverUploadService = albumCoverUploadService;
        }

        public async Task<Dictionary<string, string>> Upload(UploadModel model, IReadOnlyList<UploadFileStream> files, CancellationToken ct = default)
        {
            // filename, path
            Dictionary<string, string> dict = new Dictionary<string, string>();

            if (model.ColumnType == 1)
            {
                foreach (var file in files)
                {
                    string path = await _albumCoverUploadService.UploadCoverPathAsync(model, file.Stream, file.FileName, ct);
                    dict.Add(file.FileName, path);
                }
            }
            return dict;
        }
    }
}
