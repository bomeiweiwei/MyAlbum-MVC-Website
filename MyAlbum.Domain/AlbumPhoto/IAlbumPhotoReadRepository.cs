using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.AlbumPhoto
{
    public interface IAlbumPhotoReadRepository
    {
        Task<AlbumPhotoDto?> GetAlbumPhotoAsync(GetAlbumPhotoReq req, CancellationToken ct = default);
        Task<ResponseBase<List<AlbumPhotoDto>>> GetAlbumPhotoListAsync(PageRequestBase<GetAlbumPhotoReq> req, CancellationToken ct = default);
    }
}
