using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Album
{
    public class AlbumUpdateDto
    {
        #region 查詢用參數
        public Guid AlbumId { get; set; }
        public Guid AlbumCategoryId { get; set; }
        #endregion

        public Guid? OwnerAccountId { get; set; }

        #region 更新用參數
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? CoverPath { get; set; }
        public Status Status { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
        #endregion
    }
}
