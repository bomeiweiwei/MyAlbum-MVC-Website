using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.AlbumComment
{
    public class AlbumCommentDto
    {
        public Guid AlbumCommentId { get; set; }

        public Guid AlbumPhotoId { get; set; }

        public Guid MemberId { get; set; }

        public string Comment { get; set; } = null!;

        public DateTime ReleaseTimeUtc { get; set; }

        public bool IsChanged { get; set; }

        public Status Status { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public DateTime UpdatedAtUtc { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid UpdatedBy { get; set; }
    }
}
