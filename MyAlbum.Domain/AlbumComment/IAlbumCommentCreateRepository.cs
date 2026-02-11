using MyAlbum.Models.AlbumComment;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.AlbumComment
{
    public interface IAlbumCommentCreateRepository
    {
        Task<Guid> CreateAlbumCommentAsync(AlbumCommentCreateDto dto, CancellationToken ct = default);
    }
}
