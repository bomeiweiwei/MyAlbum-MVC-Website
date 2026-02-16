using MyAlbum.Models.Album;
using MyAlbum.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Album
{
    public interface IMemberAlbumReadService
    {
        Task<AlbumDto?> GetAlbumAsync(GetAlbumReq req, CancellationToken ct = default);
        Task<List<AlbumDto>> GetAlbumListAsync(GetAlbumListReq req, CancellationToken ct = default);
    }
}
