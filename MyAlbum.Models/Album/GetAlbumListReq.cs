using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Album
{
    public class GetAlbumListReq
    {
        public Guid? AlbumCategoryId { get; set; }
        public Guid? OwnerAccountId { get; set; }
        public string? Title { get; set; } = null;
        public DateTime? StartReleaseTimeUtc { get; set; }
        public DateTime? EndReleaseTimeUtc { get; set; }
        public Status? Status { get; set; }
    }
}
