using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.AlbumComment
{
    public class AddAlbumCommentReq
    {
        [Required]
        public Guid AlbumPhotoId { get; set; }
        [Required]
        public string Comment { get; set; }
    }
}
