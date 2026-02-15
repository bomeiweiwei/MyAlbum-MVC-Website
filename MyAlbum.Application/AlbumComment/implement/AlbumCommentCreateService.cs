using MyAlbum.Domain;
using MyAlbum.Domain.AlbumComment;
using MyAlbum.Models.AlbumComment;
using MyAlbum.Shared.Idenyity;
using MyAlbum.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumComment.implement
{
    public class AlbumCommentCreateService : BaseService, IAlbumCommentCreateService
    {
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IAlbumCommentCreateRepository _mainRepo;

        public AlbumCommentCreateService(
            IAlbumDbContextFactory factory,
            ICurrentUserAccessor currentUser,
            IAlbumCommentCreateRepository mainRepo) : base(factory)
        {
            _mainRepo = mainRepo;
            _currentUser = currentUser;
        }

        public async Task<Guid> CreateAlbumCommentAsync(CreateAlbumCommentReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();

            AlbumCommentCreateDto dto = new AlbumCommentCreateDto
            {
                AlbumCommentId = Guid.NewGuid(),
                AlbumPhotoId = req.AlbumPhotoId,
                MemberId = req.MemberId,
                Comment = req.Comment,
                ReleaseTimeUtc = DateTime.UtcNow,
                IsChanged = false,
                CreatedBy = operatorId,
                CreatedAtUtc = DateTime.UtcNow,
            };

            return await _mainRepo.CreateAlbumCommentAsync(dto, ct);
        }
    }
}
