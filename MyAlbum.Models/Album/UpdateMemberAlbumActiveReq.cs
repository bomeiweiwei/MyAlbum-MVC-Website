using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.Album
{
    public class UpdateMemberAlbumActiveReq
    {
        [Required]
        public Guid AlbumId { get; set; }
    }
}
