using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Album
{
    public interface IAlbumDataUploadService
    {
        Task<string> UploadCoverPathAsync(Guid albumId, Stream fileStream, string originalFileName, Mode mode, CancellationToken ct);
    }
}
