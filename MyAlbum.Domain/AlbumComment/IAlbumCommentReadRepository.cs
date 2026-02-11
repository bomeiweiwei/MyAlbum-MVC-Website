using MyAlbum.Models.AlbumComment;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.AlbumComment
{
    public interface IAlbumCommentReadRepository
    {
        Task<AlbumCommentDto?> GetAlbumCommentAsync(GetAlbumCommentReq req, CancellationToken ct = default);

        Task<List<AlbumCommentDto>> GetAlbumCommentListAsync(GetAlbumCommentReq req, CancellationToken ct = default);
    }
}
