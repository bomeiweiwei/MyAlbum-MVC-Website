using MyAlbum.Models.UploadFiles;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Member
{
    public interface IMemberAvatarUploadService
    {
        Task<string> UploadAvatarAsync(UploadModel model, Stream fileStream, string originalFileName, CancellationToken ct);
    }
}
