using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.AlbumPhoto
{
    public class GetTopAlbumPhotoReq
    {
        public int GetTopCount { get; set; } = 5;
        public Status? Status { get; set; }
    }
}
