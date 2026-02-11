using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.AlbumComment
{
    /// <summary>
    /// 用於帶入 LINQ 查詢條件（讀取用）
    /// </summary>
    public class GetAlbumCommentReq
    {
        public Guid? AlbumCommentId { get; set; }

        public Guid? AlbumPhotoId { get; set; }

        public Guid? MemberId { get; set; }

        public string? Comment { get; set; }

        /// <summary>
        /// 指定的日期欄位：ReleaseTimeUtc
        /// </summary>
        public DateTime? ReleaseTimeUtc { get; set; }

        public DateTime? StartReleaseTimeUtc { get; set; }

        public DateTime? EndReleaseTimeUtc { get; set; }

        public bool? IsChanged { get; set; }

        /// <summary>
        /// 系統內為 Enum
        /// </summary>
        public Status? Status { get; set; }
    }
}
