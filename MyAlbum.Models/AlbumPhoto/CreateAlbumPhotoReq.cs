using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.AlbumPhoto
{
    public class CreateAlbumPhotoReq
    {
        [Required]
        public Guid AlbumId { get; set; }
    }
}
