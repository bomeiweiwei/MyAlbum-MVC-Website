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
    public class AlbumCreateService : BaseService, IAlbumCreateService
    {
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IAlbumCreateRepository _albumCreateRepository;
        public AlbumCreateService(
            IAlbumDbContextFactory factory,
            ICurrentUserAccessor currentUser,
            IAlbumCreateRepository albumCreateRepository) : base(factory)
        {
            _albumCreateRepository = albumCreateRepository;
            _currentUser = currentUser;
        }

        public async Task<Guid> CreateAlbumAsync(CreateAlbumReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            AlbumCreateDto dto = new AlbumCreateDto
            {
                AlbumCategoryId = req.AlbumCategoryId,
                OwnerAccountId = req.OwnerAccountId,
                Title = req.Title,
                Description = req.Description,
                ReleaseTimeUtc = DateTime.UtcNow,
                TotalCommentNum = 0,
                CreatedBy = operatorId,
                CreatedAtUtc = DateTime.UtcNow,
            };
            return await _albumCreateRepository.CreateAlbumAsync(dto, ct);
        }
    }
}
