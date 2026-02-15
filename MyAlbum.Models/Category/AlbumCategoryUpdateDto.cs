using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Category
{
    public class AlbumCategoryUpdateDto
    {
        public Guid AlbumCategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public Status Status { get; set; }
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
        public Guid UpdatedBy { get; set; }
    }
}
