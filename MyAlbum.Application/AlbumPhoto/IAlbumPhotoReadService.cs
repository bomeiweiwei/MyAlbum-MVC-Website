using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumPhoto
{
    public interface IAlbumPhotoReadService
    {
        Task<AlbumPhotoDto?> GetAlbumPhotoAsync(GetAlbumPhotoReq req, CancellationToken ct = default);
        Task<ResponseBase<List<AlbumPhotoDto>>> GetAlbumPhotoListAsync(PageRequestBase<GetAlbumPhotoReq> req, CancellationToken ct = default);
    }
}
