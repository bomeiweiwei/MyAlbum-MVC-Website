using MyAlbum.Models.AlbumComment;
using MyAlbum.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.AlbumComment
{
    public interface IAlbumCommentReadRepository
    {
        Task<AlbumCommentDto?> GetAlbumCommentAsync(GetAlbumCommentReq req, CancellationToken ct = default);

        Task<ResponseBase<List<AlbumCommentDto>>> GetAlbumCommentListAsync(PageRequestBase<GetAlbumCommentListReq> req, CancellationToken ct = default);
    }
}
