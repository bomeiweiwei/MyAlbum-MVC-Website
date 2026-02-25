using MyAlbum.Models.AlbumPhoto;
using System.ComponentModel.DataAnnotations;

namespace MyAlbum.Web.Models.AlbumPhoto
{
    public class CreateAlbumPhotoWithUploadForm: CreateAlbumPhotoReq
    {
        [Required]
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
