using MyAlbum.Models.AlbumPhoto;

namespace MyAlbum.Web.Models.AlbumCategory
{
    public class AlbumCategoryViewModel
    {
        public string CategoryName { get; set; } = null!;
        public List<AlbumPhotoDto> Photos { get; set; } = new List<AlbumPhotoDto>();
    }
}
