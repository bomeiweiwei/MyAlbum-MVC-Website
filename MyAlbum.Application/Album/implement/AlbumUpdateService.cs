using MyAlbum.Domain;
using MyAlbum.Domain.Album;
using MyAlbum.Domain.Category;
using MyAlbum.Models.Album;
using MyAlbum.Models.Category;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.Idenyity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Album.implement
{
    public class AlbumUpdateService : BaseService, IAlbumUpdateService
    {
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IAlbumUpdateRepository _albumUpdateRepository;
        public AlbumUpdateService(
            IAlbumDbContextFactory factory,
            ICurrentUserAccessor currentUser,
            IAlbumUpdateRepository albumUpdateRepository) : base(factory)
        {
            _albumUpdateRepository = albumUpdateRepository;
            _currentUser = currentUser;
        }

        public async Task<bool> UpdateAlbumAsync(UpdateAlbumReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            AlbumUpdateDto dto = new AlbumUpdateDto
            {
                AlbumId = req.AlbumId,
                AlbumCategoryId = req.AlbumCategoryId,
                Title = req.Title,
                Description = req.Description,
                Status = req.Status,
                UpdatedBy = operatorId,
            };
            return await _albumUpdateRepository.UpdateAlbumAsync(dto, ct);
        }

        public async Task<bool> UpdateAlbumActiveAsync(UpdateAlbumActiveReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            AlbumUpdateActiveDto dto = new AlbumUpdateActiveDto
            {
                AlbumId = req.AlbumId,
                Status = req.Status,
                UpdateBy = operatorId,
            };
            return await _albumUpdateRepository.UpdateAlbumActiveAsync(dto, ct);
        }
    }
}
