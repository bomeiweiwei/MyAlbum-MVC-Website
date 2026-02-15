using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.AlbumPhoto
{
    public class UpdateAlbumPhotoReq
    {
        [Required]
        public Guid AlbumPhotoId { get; set; }
        [Required]
        public Guid AlbumId { get; set; }

        [Required(ErrorMessage = "排序必填")]
        public int SortOrder { get; set; }

        [Required]
        public Status Status { get; set; }
    }
}
