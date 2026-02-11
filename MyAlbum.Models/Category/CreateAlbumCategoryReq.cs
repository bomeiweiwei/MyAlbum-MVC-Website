using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.Category
{
    public class CreateAlbumCategoryReq
    {
        [Required(ErrorMessage = "類別名稱必填")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "類別名稱長度需介於 1~50")]
        public string CategoryName { get; set; } = string.Empty;
        [Required(ErrorMessage = "類別排序必填")]
        public int SortOrder { get; set; }
    }
}
