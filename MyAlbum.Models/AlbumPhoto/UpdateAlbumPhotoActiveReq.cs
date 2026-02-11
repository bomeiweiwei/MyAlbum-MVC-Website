using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.AlbumPhoto
{
    public class UpdateAlbumPhotoActiveReq
    {
        public Guid AlbumPhotoId { get; set; }
        public Status Status { get; set; }
    }
}
