using MyAlbum.Models.AlbumPhoto;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.AlbumComment
{
    public class AlbumPhotoCommentsDto
    {
        public AlbumPhotoDto? Photo { get; set; }
        public List<AlbumCommentDto> Comments { get; set; } = new List<AlbumCommentDto>();
    }
}
