using System.ComponentModel.DataAnnotations;

namespace MyAlbum.Web.Models.AlbumPhoto
{
    public class CreateAlbumPhotosViewModel
    {
        [Required]
        public Guid AlbumId { get; set; }

        [Required(ErrorMessage = "請選擇至少 1 張圖片")]
        public List<IFormFile> Files { get; set; } = new();
    }
}
