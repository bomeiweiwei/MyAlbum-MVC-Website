using MyAlbum.Models.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.Category
{
    public interface IAlbumCategoryReadRepository
    {
        Task<AlbumCategoryDto?> GetAlbumCategoryAsync(GetAlbumCategoryReq req, CancellationToken ct = default);

        Task<List<AlbumCategoryDto>> GetAlbumCategoryListAsync(GetAlbumCategoryReq req, CancellationToken ct = default);
    }
}
