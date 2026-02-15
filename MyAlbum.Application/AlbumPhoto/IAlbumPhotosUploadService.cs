using MyAlbum.Models.UploadFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumPhoto
{
    public interface IAlbumPhotosUploadService
    {
        Task<string> UploadPathAsync(UploadModel model, Stream fileStream, string originalFileName, CancellationToken ct);
    }
}
