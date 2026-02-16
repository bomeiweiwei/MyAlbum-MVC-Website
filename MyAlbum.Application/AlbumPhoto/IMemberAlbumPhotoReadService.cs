using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumPhoto
{
    public interface IMemberAlbumPhotoReadService
    {
        Task<List<AlbumPhotoDto>> GetAlbumPhotoListAsync(GetAlbumPhotoReq req, CancellationToken ct = default);
    }
}
