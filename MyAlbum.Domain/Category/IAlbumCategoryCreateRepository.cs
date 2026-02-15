using MyAlbum.Models.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.Category
{
    public interface IAlbumCategoryCreateRepository
    {
        Task<Guid> CreateAlbumCategoryAsync(AlbumCategoryCreateDto dto, CancellationToken ct = default);
    }
}
