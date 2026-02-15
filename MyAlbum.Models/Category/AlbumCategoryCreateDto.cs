using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Category
{
    public class AlbumCategoryCreateDto
    {
        public Guid AlbumCategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
