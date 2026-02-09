using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Album
{
    public class AlbumUpdateActiveDto
    {
        #region 查詢用參數
        public Guid AlbumId { get; set; }
        public Guid AlbumCategoryId { get; set; }
        #endregion

        public Guid OwnerAccountId { get; set; }

        public Status Status { get; set; }
        public Guid UpdateBy { get; set; }
    }
}
