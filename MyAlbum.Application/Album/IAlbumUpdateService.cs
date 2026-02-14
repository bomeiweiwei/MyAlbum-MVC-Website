using MyAlbum.Models.Album;
using MyAlbum.Models.Category;
using MyAlbum.Models.UploadFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Album
{
    public interface IAlbumUpdateService
    {
        Task<bool> UpdateAlbumAsync(UpdateAlbumReq req, IReadOnlyList<UploadFileStream> files, CancellationToken ct = default);

        Task<bool> UpdateAlbumActiveAsync(UpdateAlbumActiveReq req, CancellationToken ct = default);
    }
}
