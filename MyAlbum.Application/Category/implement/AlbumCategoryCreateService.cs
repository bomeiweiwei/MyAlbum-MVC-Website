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
    public class AlbumCategoryCreateService : BaseService, IAlbumCategoryCreateService
    {
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IAlbumCategoryCreateRepository _albumCategoryCreateRepository;
        public AlbumCategoryCreateService(
            IAlbumDbContextFactory factory,
            ICurrentUserAccessor currentUser,
            IAlbumCategoryCreateRepository albumCategoryCreateRepository) : base(factory)
        {
            _albumCategoryCreateRepository = albumCategoryCreateRepository;
            _currentUser = currentUser;
        }

        public async Task<Guid> CreateAlbumCategoryAsync(CreateAlbumCategoryReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            AlbumCategoryCreateDto dto = new AlbumCategoryCreateDto
            {
                AlbumCategoryId = Guid.NewGuid(),
                CategoryName = req.CategoryName,
                SortOrder = req.SortOrder,
                CreatedBy = operatorId
            };
            return await _albumCategoryCreateRepository.CreateAlbumCategoryAsync(dto, ct);
        }
    }
}
