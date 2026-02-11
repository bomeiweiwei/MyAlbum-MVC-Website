using MyAlbum.Models.Album;
using MyAlbum.Models.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Album
{
    public interface IAlbumUpdateService
    {
        Task<bool> UpdateAlbumAsync(UpdateAlbumReq req, CancellationToken ct = default);

        Task<bool> UpdateAlbumActiveAsync(UpdateAlbumActiveReq req, CancellationToken ct = default);
    }
}
