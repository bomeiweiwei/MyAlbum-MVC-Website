using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.AlbumPhoto
{
    public sealed class AlbumPhotoCreateDto
    {
        public Guid AlbumPhotoId { get; set; }

        public Guid AlbumId { get; set; }

        public string FilePath { get; set; } = null!;

        public string? OriginalFileName { get; set; }

        public string? ContentType { get; set; }

        public long FileSizeBytes { get; set; }

        public int SortOrder { get; set; }

        public int CommentNum { get; set; }

        public Status Status { get; set; } = Status.Active;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public Guid CreatedBy { get; set; }
    }
}
