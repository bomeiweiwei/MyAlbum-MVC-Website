using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.AlbumPhoto
{
    public class AlbumPhotoUpdateActiveDto
    {
        public Guid AlbumPhotoId { get; set; }
        public Status Status { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedAtUtc { get; set; }= DateTime.UtcNow;

        public bool UpdateByMember { get; set; } = true;
        public Guid OwnerAccountId { get; set; }
    }
}
