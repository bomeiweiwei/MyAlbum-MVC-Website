using MyAlbum.Domain;
using MyAlbum.Domain.Category;
using MyAlbum.Models.Category;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.Idenyity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Category.implement
{
    public class AlbumCategoryUpdateService : BaseService, IAlbumCategoryUpdateService
    {
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IAlbumCategoryUpdateRepository _albumCategoryUpdateRepository;
        public AlbumCategoryUpdateService(
            IAlbumDbContextFactory factory,
            ICurrentUserAccessor currentUse,
            IAlbumCategoryUpdateRepository albumCategoryUpdateRepository) : base(factory)
        {
            this._albumCategoryUpdateRepository = albumCategoryUpdateRepository;
            this._currentUser = currentUse;
        }

        public async Task<bool> UpdateAlbumCategoryAsync(UpdateAlbumCategoryReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            AlbumCategoryUpdateDto dto = new AlbumCategoryUpdateDto
            {
                AlbumCategoryId = req.AlbumCategoryId,
                CategoryName = req.CategoryName,
                SortOrder = req.SortOrder,
                Status = req.Status,
                UpdatedAtUtc = DateTime.UtcNow,
                UpdatedBy = operatorId
            };
            return await _albumCategoryUpdateRepository.UpdateAlbumCategoryAsync(dto, ct);
        }

        public async Task<bool> UpdateAlbumCategoryActiveAsync(UpdateAlbumCategoryActiveReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            AlbumCategoryUpdateActiveDto dto = new AlbumCategoryUpdateActiveDto
            {
                AlbumCategoryId = req.AlbumCategoryId,
                Status = req.Status,
                UpdatedAtUtc = DateTime.UtcNow,
                UpdatedBy = operatorId
            };
            return await _albumCategoryUpdateRepository.UpdateAlbumCategoryActiveAsync(dto, ct);
        }
    }
}
