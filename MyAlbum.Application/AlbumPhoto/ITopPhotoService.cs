using MyAlbum.Models.AlbumPhoto;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumPhoto
{
    public interface ITopPhotoService
    {
        Task<List<AlbumPhotoDto>> GetTopPhotos(GetTopAlbumPhotoReq req, CancellationToken ct = default);
    }
}
