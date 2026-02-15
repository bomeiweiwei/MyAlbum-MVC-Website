using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.AlbumComment
{
    public sealed class AlbumCommentUpdateDto
    {
        // 檢查欄位（更新前需確認資料存在）
        public Guid AlbumCommentId { get; set; }
        public Guid AlbumPhotoId { get; set; }
        public Guid MemberId { get; set; }

        // 可更新欄位
        public string Comment { get; set; } = null!;
        public bool IsChanged { get; set; } = true;
        public Status Status { get; set; }

        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
        public Guid UpdatedBy { get; set; }

        public bool UpdateByMember { get; set; } = true;
    }
}
