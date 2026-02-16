using MyAlbum.Models.AlbumPhoto;

namespace MyAlbum.Web.Models.AlbumPhoto
{
    public class UpdateAlbumPhotoWithUploadForm: UpdateAlbumPhotoReq
    {
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
