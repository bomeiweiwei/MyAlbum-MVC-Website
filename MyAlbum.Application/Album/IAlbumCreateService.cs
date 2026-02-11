using MyAlbum.Models.Album;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Album
{
    public interface IAlbumCreateService
    {
        Task<Guid> CreateAlbumAsync(CreateAlbumReq req, CancellationToken ct = default);
    }
}
