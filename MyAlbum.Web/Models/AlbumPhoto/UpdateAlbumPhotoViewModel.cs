using MyAlbum.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyAlbum.Web.Models.AlbumPhoto
{
    public class UpdateAlbumPhotoViewModel
    {
        [Required]
        public Guid AlbumId { get; set; }

        [Required]
        public Guid AlbumPhotoId { get; set; }

        public IFormFile? File { get; set; } // 可不選

        [Required]
        public Status Status { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "排序 必須為 0 以上")]
        public int SortOrder { get; set; } 
    }
}
