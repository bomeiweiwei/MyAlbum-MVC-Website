using MyAlbum.Domain;
using MyAlbum.Domain.Category;
using MyAlbum.Models.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Category.implement
{
    public class AlbumCategoryReadService : BaseService, IAlbumCategoryReadService
    {
        private readonly IAlbumCategoryReadRepository _albumCategoryReadRepository;
        public AlbumCategoryReadService(
            IAlbumDbContextFactory factory,
            IAlbumCategoryReadRepository albumCategoryReadRepository
            ) : base(factory)
        {
            _albumCategoryReadRepository = albumCategoryReadRepository;
        }

        public async Task<AlbumCategoryDto?> GetAlbumCategoryAsync(GetAlbumCategoryReq req, CancellationToken ct = default)
        {
            return await _albumCategoryReadRepository.GetAlbumCategoryAsync(req, ct);
        }

        public async Task<List<AlbumCategoryDto>> GetAlbumCategoryListAsync(GetAlbumCategoryReq req, CancellationToken ct = default)
        {
            return await _albumCategoryReadRepository.GetAlbumCategoryListAsync(req, ct);
        }
    }
}
