using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Album
{
    public class AlbumDto
    {
        public Guid AlbumId { get; set; }
        public Guid AlbumCategoryId { get; set; }
        public Guid OwnerAccountId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; } = null;
        //public string? CoverPath { get; set; } = null;
        public DateTime ReleaseTimeUtc { get; set; }
        public int TotalCommentNum { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        public string PublicCoverUrl { get; set; }
        public string OwnerName { get; set; }
        public string CategoryName { get; set; }
    }
}
