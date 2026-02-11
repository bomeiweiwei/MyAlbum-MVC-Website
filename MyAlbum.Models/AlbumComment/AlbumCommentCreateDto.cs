using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.AlbumComment
{
    public class AlbumCommentCreateDto
    {
        public Guid AlbumCommentId { get; set; }

        public Guid AlbumPhotoId { get; set; }

        public Guid MemberId { get; set; }

        public string Comment { get; set; } = null!;

        public DateTime ReleaseTimeUtc { get; set; }

        public bool IsChanged { get; set; }

        public Status Status { get; set; } = Status.Active;

        /// <summary>
        /// 指定的日期欄位：CreatedAtUtc
        /// 若未指定，預設 DateTime.UtcNow
        /// </summary>
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public Guid CreatedBy { get; set; }
    }
}
