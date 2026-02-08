using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Member
{
    public interface IMemberDataUploadService
    {
        Task<string> UploadAvatarAsync(Guid memberId, Stream fileStream, string originalFileName, CancellationToken ct);
    }
}
