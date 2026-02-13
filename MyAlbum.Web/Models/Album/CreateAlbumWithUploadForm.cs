using MyAlbum.Models.Album;
using System.ComponentModel.DataAnnotations;

namespace MyAlbum.Web.Models.Album
{
    public class CreateAlbumWithUploadForm: CreateAlbumReq
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
