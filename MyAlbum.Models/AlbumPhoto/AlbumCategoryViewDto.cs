using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.AlbumPhoto
{
    public class AlbumCategoryViewDto
    {
        public string CategoryName { get; set; } = null!;
        public List<AlbumPhotoDto> Photos { get; set; } = new List<AlbumPhotoDto>();
    }
}
