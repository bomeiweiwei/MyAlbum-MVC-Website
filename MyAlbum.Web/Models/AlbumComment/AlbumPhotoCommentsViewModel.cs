using MyAlbum.Models.AlbumComment;
using MyAlbum.Models.AlbumPhoto;

namespace MyAlbum.Web.Models.AlbumComment
{
    public class AlbumPhotoCommentsViewModel
    {
        public AlbumPhotoDto Photo { get; }
        public List<AlbumCommentDto> Comments { get; }

        public AlbumPhotoCommentsViewModel(
            AlbumPhotoDto photo,
            List<AlbumCommentDto>? comments = null)
        {
            Photo = photo ?? throw new ArgumentNullException(nameof(photo));
            Comments = comments ?? new List<AlbumCommentDto>();
        }
    }
}
