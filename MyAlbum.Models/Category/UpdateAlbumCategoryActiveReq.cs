using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Category
{
    public class UpdateAlbumCategoryActiveReq
    {
        public Guid AlbumCategoryId { get; set; }
        public Status Status { get; set; }
    }
}
