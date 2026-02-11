using MyAlbum.Models.AlbumComment;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumComment
{
    public interface IAlbumCommentReadService
    {
        /// <summary>
        /// 取得單筆（可為 null）
        /// </summary>
        Task<AlbumCommentDto?> GetAlbumCommentAsync(GetAlbumCommentReq req, CancellationToken ct = default);

        /// <summary>
        /// 取得清單
        /// </summary>
        Task<List<AlbumCommentDto>> GetAlbumCommentListAsync(GetAlbumCommentReq req, CancellationToken ct = default);
    }
}
