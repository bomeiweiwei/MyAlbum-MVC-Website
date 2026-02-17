using MyAlbum.Models.AlbumComment;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumComment
{
    public interface IAlbumPhotoCommentReadService
    {
        Task<AlbumPhotoCommentsDto> GetAlbumPhotoComments(Guid Id);
    }
}
