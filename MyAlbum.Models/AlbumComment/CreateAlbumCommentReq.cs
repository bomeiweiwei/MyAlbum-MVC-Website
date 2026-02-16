using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.AlbumComment
{
    public class CreateAlbumCommentReq
    {
        [Required]
        public Guid AlbumPhotoId { get; set; }
        [Required]
        public Guid MemberId { get; set; }
        [Required]
        public string Comment { get; set; } = null!;
    }
}
