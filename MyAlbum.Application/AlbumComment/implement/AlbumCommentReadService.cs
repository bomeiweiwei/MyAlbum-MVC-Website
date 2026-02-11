using MyAlbum.Domain;
using MyAlbum.Domain.AlbumComment;
using MyAlbum.Models.AlbumComment;
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

        public async Task<List<AlbumCommentDto>> GetAlbumCommentListAsync(GetAlbumCommentReq req, CancellationToken ct = default)
        {
            return await _mainRepo.GetAlbumCommentListAsync(req, ct);
        }
    }
}
