using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAlbum.Models.Category
{
    public class UpdateAlbumCategoryActiveReq
    {
        [Required]
        public Guid AlbumCategoryId { get; set; }
        [Required]
        public Status Status { get; set; }
    }
}
