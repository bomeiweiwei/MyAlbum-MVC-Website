using MyAlbum.Domain;
using MyAlbum.Domain.AlbumComment;
using MyAlbum.Models.AlbumComment;
using MyAlbum.Models.Base;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.Idenyity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumComment.implement
{
    public class MemberAlbumCommentReadService : BaseService, IMemberAlbumCommentReadService
    {
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IAlbumCommentReadRepository _read;

        public MemberAlbumCommentReadService(
            IAlbumDbContextFactory factory,
            ICurrentUserAccessor currentUser,
            IAlbumCommentReadRepository read
        ) : base(factory)
        {
            _read = read;
            _currentUser = currentUser;
        }

        public async Task<List<AlbumCommentDto>> GetAlbumCommentListAsync(GetAlbumCommentListReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();

            req.OwnerAccountId = operatorId;

            var pageReq = new PageRequestBase<GetAlbumCommentListReq>()
            {
                pageIndex = 1,
                pageSize = 99999,
                Data = req
            };
            var resp = await _read.GetAlbumCommentListAsync(pageReq, ct);
            var list = resp.Data;
            return list;
        }
    }
}
