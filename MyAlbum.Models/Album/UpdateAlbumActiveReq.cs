using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.Album
{
    public class UpdateAlbumActiveReq
    {
        [Required]
        public Guid AlbumId { get; set; }
        [Required]
        public Status Status { get; set; }
        [Required]
        public Guid OwnerAccountId { get; set; }
    }
}
