using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.AlbumPhoto
{
    public class UpdateMemberAlbumPhotoActiveReq
    {
        [Required] 
        public Guid AlbumId { get; set; }
        [Required] 
        public Guid AlbumPhotoId { get; set; }
    }
}
