using MyAlbum.Models.Album;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.Album
{
    public interface IAlbumUpdateRepository
    {
        Task<bool> UpdateAlbumAsync(AlbumUpdateDto dto, CancellationToken ct = default);

        Task<bool> UpdateAlbumActiveAsync(AlbumUpdateActiveDto dto, CancellationToken ct = default);
    }
}
