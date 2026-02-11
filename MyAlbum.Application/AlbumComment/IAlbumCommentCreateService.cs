using MyAlbum.Models.AlbumComment;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumComment
{
    public interface IAlbumCommentCreateService
    {
        /// <summary>
        /// 建立 AlbumComment，回傳新增資料的 PK：AlbumCommentId
        /// </summary>
        Task<Guid> CreateAlbumCommentAsync(CreateAlbumCommentReq req, CancellationToken ct = default);
    }
}
