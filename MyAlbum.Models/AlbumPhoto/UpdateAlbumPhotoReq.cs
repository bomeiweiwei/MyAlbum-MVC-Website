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

        [Required(ErrorMessage = "檔案路徑必填")]
        public string FilePath { get; set; }

        public string? OriginalFileName { get; set; }

        public string? ContentType { get; set; }

        public long FileSizeBytes { get; set; }

        [Required(ErrorMessage = "排序必填")]
        public int SortOrder { get; set; }

        public Status Status { get; set; }
    }
}
