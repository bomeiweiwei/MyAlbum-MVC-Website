using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Album
{
    public class AlbumCreateDto
    {
        public Guid AlbumId { get; set; } = Guid.NewGuid();
        public Guid AlbumCategoryId { get; set; }
        public Guid OwnerAccountId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? CoverPath { get; set; }
        public DateTime ReleaseTimeUtc { get; set; } = DateTime.UtcNow;
        public int TotalCommentNum { get; set; }
        public Status Status { get; set; } = Status.Active;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public Guid CreatedBy { get; set; }
    }
}
