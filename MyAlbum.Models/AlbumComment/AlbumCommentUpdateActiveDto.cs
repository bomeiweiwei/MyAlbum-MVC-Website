using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.AlbumComment
{
    public sealed class AlbumCommentUpdateActiveDto
    {
        // 檢查欄位（更新前需確認資料存在）
        public Guid AlbumCommentId { get; set; }

        // 可更新欄位
        public Status Status { get; set; }

        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
        public Guid UpdatedBy { get; set; }
    }
}
