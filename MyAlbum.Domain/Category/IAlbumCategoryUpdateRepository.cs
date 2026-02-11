using MyAlbum.Models.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.Category
{
    public interface IAlbumCategoryUpdateRepository
    {
        Task<bool> UpdateAlbumCategoryAsync(AlbumCategoryUpdateDto dto, CancellationToken ct = default);

        Task<bool> UpdateAlbumCategoryActiveAsync(AlbumCategoryUpdateActiveDto dto, CancellationToken ct = default);
    }
}
