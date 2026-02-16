using MyAlbum.Models.Album;
using System.ComponentModel.DataAnnotations;

namespace MyAlbum.Web.Models.Album
{
    public class CreateAlbumWithUploadForm: CreateAlbumReq
    {
        [Required]
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
