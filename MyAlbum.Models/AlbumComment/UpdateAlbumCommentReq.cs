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
        public Guid AlbumPhotoId { get; set; }

        [Required]
        public Guid MemberId { get; set; }

        [Required]
        public string Comment { get; set; } = null!;

        // 文件要求：遇到 Status 關鍵字，型別必須是 enum Status
        [Required]
        public Status Status { get; set; }
    }
}
