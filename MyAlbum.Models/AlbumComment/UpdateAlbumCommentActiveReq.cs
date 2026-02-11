using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.AlbumComment
{
    public class UpdateAlbumCommentActiveReq
    {
        [Required]
        public Guid AlbumCommentId { get; set; }

        [Required]
        public Status Status { get; set; }
    }
}
