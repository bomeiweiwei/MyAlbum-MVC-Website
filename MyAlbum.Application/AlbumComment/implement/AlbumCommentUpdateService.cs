using MyAlbum.Domain;
using MyAlbum.Domain.AlbumComment;
using MyAlbum.Models.AlbumComment;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.Idenyity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumComment.implement
{
    public class AlbumCommentUpdateService : BaseService, IAlbumCommentUpdateService
    {
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IAlbumCommentUpdateRepository _mainRepo;

        public AlbumCommentUpdateService(
            IAlbumDbContextFactory factory,
            ICurrentUserAccessor currentUser,
            IAlbumCommentUpdateRepository mainRepo) : base(factory)
        {
            _mainRepo = mainRepo;
            _currentUser = currentUser;
        }

        public async Task<bool> UpdateAlbumCommentAsync(UpdateAlbumCommentReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            var type = _currentUser.GetRequiredAccountType();

            AlbumCommentUpdateDto dto = new AlbumCommentUpdateDto();
            dto.AlbumCommentId = req.AlbumCommentId;
            dto.MemberId = req.MemberId;
            dto.Comment = req.Comment;
            dto.IsChanged = true;
            dto.Status = req.Status;
            dto.UpdatedBy = operatorId;
            dto.UpdatedAtUtc = DateTime.UtcNow;
            if (type == AccountType.Member)
            {
                // 覆蓋
                dto.MemberId = operatorId;
                dto.UpdateByMember = true;
            }
            else
            {
                dto.UpdateByMember = false;
            }
            return await _mainRepo.UpdateAlbumCommentAsync(dto, ct);
        }

        public async Task<bool> UpdateAlbumCommentActiveAsync(UpdateAlbumCommentActiveReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();
            var type = _currentUser.GetRequiredAccountType();

            AlbumCommentUpdateActiveDto dto = new AlbumCommentUpdateActiveDto();
            dto.AlbumCommentId = req.AlbumCommentId; // PK
            dto.Status = req.Status;
            dto.UpdatedBy = operatorId;
            dto.UpdatedAtUtc = DateTime.UtcNow;
            if (type == AccountType.Member)
            {
                // 覆蓋
                dto.MemberId = operatorId;
                dto.UpdateByMember = true;
            }
            else
            {
                dto.UpdateByMember = false;
            }
            return await _mainRepo.UpdateAlbumCommentActiveAsync(dto, ct);
        }
    }
}
