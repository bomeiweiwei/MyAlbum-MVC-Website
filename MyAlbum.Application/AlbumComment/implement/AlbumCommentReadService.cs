using MyAlbum.Domain;
using MyAlbum.Domain.AlbumComment;
using MyAlbum.Models.AlbumComment;
using MyAlbum.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumComment.implement
{
    public class AlbumCommentReadService : BaseService, IAlbumCommentReadService
    {
        private readonly IAlbumCommentReadRepository _mainRepo;

        public AlbumCommentReadService(
            IAlbumDbContextFactory factory,
            IAlbumCommentReadRepository mainrepo
        ) : base(factory)
        {
            _mainRepo = mainrepo;
        }

        public async Task<AlbumCommentDto?> GetAlbumCommentAsync(GetAlbumCommentReq req, CancellationToken ct = default)
        {
            return await _mainRepo.GetAlbumCommentAsync(req, ct);
        }

        public async Task<ResponseBase<List<AlbumCommentDto>>> GetAlbumCommentListAsync(PageRequestBase<GetAlbumCommentListReq> req, CancellationToken ct = default)
        {
            req.Data.StartReleaseTimeUtc = req.Data.StartLocalMs.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(req.Data.StartLocalMs.Value).UtcDateTime : null;
            req.Data.EndReleaseTimeUtc = req.Data.EndLocalMs.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(req.Data.EndLocalMs.Value).UtcDateTime : null;

            return await _mainRepo.GetAlbumCommentListAsync(req, ct);
        }
    }
}
