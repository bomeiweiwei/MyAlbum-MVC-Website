using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.Album
{
    public class CreateAlbumReq
    {
        [Required]
        public Guid AlbumCategoryId { get; set; }

        [Required]
        public Guid OwnerAccountId { get; set; }

        [Required(ErrorMessage = "標題必填")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "標題必填長度需介於 1~100")]
        public string Title { get; set; }

        public string? Description { get; set; }

        // 檔案上傳用
        public byte[]? FileBytes { get; set; }
        public string? FileName { get; set; }
    }
}
