using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.AlbumComment
{
    public class UpdateAlbumCommentReq
    {
        [Required]
        public Guid AlbumCommentId { get; set; }

        [Required]
        public Guid MemberId { get; set; }

        [Required]
        public string Comment { get; set; } = null!;

        public Status Status { get; set; }
    }
}
