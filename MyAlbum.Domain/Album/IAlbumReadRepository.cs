using MyAlbum.Models.Album;
using MyAlbum.Models.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.Album
{
    public interface IAlbumReadRepository
    {
        Task<AlbumDto?> GetAlbumAsync(GetAlbumReq req, CancellationToken ct = default);

        Task<List<AlbumDto>> GetAlbumListAsync(GetAlbumReq req, CancellationToken ct = default);
    }
}
