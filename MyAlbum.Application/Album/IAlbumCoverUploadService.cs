using MyAlbum.Models.UploadFiles;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Album
{
    public interface IAlbumCoverUploadService
    {
        Task<string> UploadCoverPathAsync(UploadModel model, Stream fileStream, string originalFileName, CancellationToken ct);
    }
}
