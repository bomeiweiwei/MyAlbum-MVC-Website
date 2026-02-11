using MyAlbum.Models.AlbumPhoto;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.AlbumPhoto
{
    public interface IAlbumPhotoReadRepository
    {
        Task<AlbumPhotoDto?> GetAlbumPhotoAsync(GetAlbumPhotoReq req, CancellationToken ct = default);
        Task<List<AlbumPhotoDto>> GetAlbumPhotoListAsync(GetAlbumPhotoReq req, CancellationToken ct = default);
    }
}
