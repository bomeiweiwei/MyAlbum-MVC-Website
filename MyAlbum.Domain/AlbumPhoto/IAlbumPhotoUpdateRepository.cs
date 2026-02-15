using MyAlbum.Models.AlbumPhoto;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.AlbumPhoto
{
    public interface IAlbumPhotoUpdateRepository
    {
        Task<bool> UpdateAlbumPhotoAsync(AlbumPhotoUpdateDto dto, CancellationToken ct = default);

        Task<bool> UpdateAlbumPhotoActiveAsync(AlbumPhotoUpdateActiveDto dto, CancellationToken ct = default);
    }
}
