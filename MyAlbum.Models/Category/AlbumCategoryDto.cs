using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Category
{
    public class AlbumCategoryDto
    {
        public Guid AlbumCategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public Status Status { get; set; }
    }
}
