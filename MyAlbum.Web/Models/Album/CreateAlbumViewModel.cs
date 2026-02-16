using MyAlbum.Models.Category;
using System.ComponentModel.DataAnnotations;

namespace MyAlbum.Web.Models.Album
{
    public class CreateAlbumViewModel
    {
        [Required(ErrorMessage = "類別必填")]
        public Guid AlbumCategoryId { get; set; }

        [Required(ErrorMessage = "標題必填")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "標題必填長度需介於 1~100")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "請選擇封面")]
        public IFormFile File { get; set; } = null!;

        public List<AlbumCategoryDto> CategoryList { get; set; } = new();
    }
}
