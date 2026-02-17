using MyAlbum.Models.AlbumPhoto;

namespace MyAlbum.Web.Models.TopImgComment
{
    public class TopPhotoCommentViewModel
    {
        public List<AlbumPhotoDto> Photos { get; set; } = new List<AlbumPhotoDto>();
    }
}
