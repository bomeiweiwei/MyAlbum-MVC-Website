using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Models.UploadFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumPhoto
{
    public interface IAlbumPhotoCreateService
    {
        Task<Guid> CreateAlbumPhotoAsync(CreateAlbumPhotoReq req, IReadOnlyList<UploadFileStream> files, CancellationToken ct = default);
    }
}
