using MyAlbum.Models.AlbumPhoto;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumPhoto
{
    public interface ICategoryPhotoReadService
    {
        Task<AlbumCategoryViewDto> GetAlbumCategoryData(Guid Id, CancellationToken ct = default);
    }
}
