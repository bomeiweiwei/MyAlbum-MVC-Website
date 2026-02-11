using MyAlbum.Models.Album;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.Album
{
    public interface IAlbumCreateRepository
    {
        Task<Guid> CreateAlbumAsync(AlbumCreateDto dto, CancellationToken ct = default);
    }
}
