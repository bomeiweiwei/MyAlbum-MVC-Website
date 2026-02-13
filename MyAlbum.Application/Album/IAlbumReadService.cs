using MyAlbum.Models.Album;
using MyAlbum.Models.Base;
using MyAlbum.Models.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Album
{
    public interface IAlbumReadService
    {
        Task<AlbumDto?> GetAlbumAsync(GetAlbumReq req, CancellationToken ct = default);

        Task<ResponseBase<List<AlbumDto>>> GetAlbumListAsync(PageRequestBase<GetAlbumListReq> req, CancellationToken ct = default);
    }
}
