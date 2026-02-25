using MyAlbum.Models.Base;
using MyAlbum.Models.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Category
{
    public interface IAlbumCategoryReadService
    {
        Task<AlbumCategoryDto?> GetAlbumCategoryAsync(GetAlbumCategoryReq req, CancellationToken ct = default);

        Task<ResponseBase<List<AlbumCategoryDto>>> GetAlbumCategoryListAsync(PageRequestBase<GetAlbumCategoryListReq> req, CancellationToken ct = default);

        Task<List<AlbumCategoryDto>> GetAlbumCategoryItemListAsync(GetAlbumCategoryListReq req, CancellationToken ct = default);
    }
}
