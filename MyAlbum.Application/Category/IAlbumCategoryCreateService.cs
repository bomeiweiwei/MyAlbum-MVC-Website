using MyAlbum.Models.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Category
{
    public interface IAlbumCategoryCreateService
    {
        Task<Guid> CreateAlbumCategoryAsync(CreateAlbumCategoryReq req, CancellationToken ct = default);
    }
}
