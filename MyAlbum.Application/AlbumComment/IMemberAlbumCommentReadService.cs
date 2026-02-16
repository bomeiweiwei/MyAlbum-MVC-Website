using MyAlbum.Models.AlbumComment;
using MyAlbum.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumComment
{
    public interface IMemberAlbumCommentReadService
    {
        Task<List<AlbumCommentDto>> GetAlbumCommentListAsync(GetAlbumCommentListReq req, CancellationToken ct = default);
    }
}
