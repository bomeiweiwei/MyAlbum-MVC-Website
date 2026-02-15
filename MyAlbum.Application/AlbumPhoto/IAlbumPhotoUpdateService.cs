using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Models.UploadFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumPhoto
{
    public interface IAlbumPhotoUpdateService
    {
        Task<bool> UpdateAlbumPhotoAsync(UpdateAlbumPhotoReq req, IReadOnlyList<UploadFileStream> files, CancellationToken ct = default);

        Task<bool> UpdateAlbumPhotoActiveAsync(UpdateAlbumPhotoActiveReq req, CancellationToken ct = default);
    }
}
