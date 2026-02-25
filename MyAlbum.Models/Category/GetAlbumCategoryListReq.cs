using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Category
{
    public class GetAlbumCategoryListReq
    {
        public string? CategoryName { get; set; }
        public Status? Status { get; set; }
    }
}
