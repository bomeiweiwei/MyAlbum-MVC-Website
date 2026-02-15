using MyAlbum.Models.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Category
{
    public interface IAlbumCategoryUpdateService
    {
        Task<bool> UpdateAlbumCategoryAsync(UpdateAlbumCategoryReq req, CancellationToken ct = default);

        Task<bool> UpdateAlbumCategoryActiveAsync(UpdateAlbumCategoryActiveReq req, CancellationToken ct = default);
    }
}
