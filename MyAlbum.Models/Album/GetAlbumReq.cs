using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Album
{
    public class GetAlbumReq
    {
        public Guid? AlbumId { get; set; }
        public Guid? AlbumCategoryId { get; set; }
        public Guid? OwnerAccountId { get; set; }
        public string? Title { get; set; } = null;
        public DateTime? ReleaseTimeUtc { get; set; }
        public DateTime? StartReleaseTimeUtc { get; set; }
        public DateTime? EndReleaseTimeUtc { get; set; }
        public Status? Status { get; set; }
    }
}
