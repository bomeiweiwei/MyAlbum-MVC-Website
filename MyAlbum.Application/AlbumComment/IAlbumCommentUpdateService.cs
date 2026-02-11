using MyAlbum.Models.AlbumComment;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumComment
{
    public interface IAlbumCommentUpdateService
    {
        Task<bool> UpdateAlbumCommentAsync(UpdateAlbumCommentReq req, CancellationToken ct = default);
        Task<bool> UpdateAlbumCommentActiveAsync(UpdateAlbumCommentActiveReq req, CancellationToken ct = default);
    }
}
