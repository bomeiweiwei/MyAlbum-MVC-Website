using MyAlbum.Models.AlbumPhoto;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumPhoto
{
    public interface IAlbumPhotoUpdateService
    {
        Task<bool> UpdateAlbumPhotoAsync(UpdateAlbumPhotoReq req, CancellationToken ct = default);

        Task<bool> UpdateAlbumPhotoActiveAsync(UpdateAlbumPhotoActiveReq req, CancellationToken ct = default);
    }
}
