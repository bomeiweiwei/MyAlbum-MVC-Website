using MyAlbum.Models.Album;

namespace MyAlbum.Web.Models.Album
{
    public class UpdateAlbumWithUploadForm: UpdateAlbumReq
    {
        public IFormFile? File { get; set; }
    }
}
