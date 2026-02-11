using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Category
{
    public class GetAlbumCategoryReq
    {
        public Guid AlbumCategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
