using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        public long? StartLocalMs { get; set; }
        public long? EndLocalMs { get; set; }

        [BindNever]
        public DateTime? StartReleaseTimeUtc { get; set; }
        [BindNever]
        public DateTime? EndReleaseTimeUtc { get; set; }

        public Status? Status { get; set; }

        public string? OwnerName { get; set; }
    }
}
