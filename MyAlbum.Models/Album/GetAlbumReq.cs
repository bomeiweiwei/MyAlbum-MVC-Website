using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Album
{
    public class GetAlbumReq
    {
        public Guid AlbumId { get; set; }
        public Guid? AlbumCategoryId { get; set; }
        public Guid? OwnerAccountId { get; set; }
        public Status? Status { get; set; }
    }
}
