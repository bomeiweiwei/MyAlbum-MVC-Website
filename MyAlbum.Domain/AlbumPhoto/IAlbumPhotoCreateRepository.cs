using MyAlbum.Models.AlbumPhoto;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.AlbumPhoto
{
    public interface IAlbumPhotoCreateRepository
    {
        Task<Guid> CreateAlbumPhotoAsync(AlbumPhotoCreateDto dto, CancellationToken ct = default);
    }
}
