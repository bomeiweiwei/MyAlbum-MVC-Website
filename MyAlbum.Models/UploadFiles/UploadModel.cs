using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.UploadFiles
{
    public class UploadModel
    {
        /// <summary>
        /// 表格的PK
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 自定義欄位
        /// </summary>
        public int ColumnType { get; set; }

        /// <summary>
        /// 表格
        /// </summary>
        public EntityUploadType EntityUploadType { get; set; }
    }
}
