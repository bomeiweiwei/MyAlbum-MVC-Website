using MyAlbum.Models.Album;

namespace MyAlbum.Web.Models.Album
{
    public class UpdateAlbumWithUploadForm: UpdateAlbumReq
    {
        public List<IFormFile> Files { get; set; } = new();
    }
}
