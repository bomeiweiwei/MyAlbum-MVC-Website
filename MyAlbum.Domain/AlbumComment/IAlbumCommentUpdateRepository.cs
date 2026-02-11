using MyAlbum.Models.AlbumComment;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.AlbumComment
{
    public interface IAlbumCommentUpdateRepository
    {
        Task<bool> UpdateAlbumCommentAsync(AlbumCommentUpdateDto dto, CancellationToken ct = default);
        Task<bool> UpdateAlbumCommentActiveAsync(AlbumCommentUpdateActiveDto dto, CancellationToken ct = default);
    }
}
