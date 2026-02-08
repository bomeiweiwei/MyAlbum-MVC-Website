using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Uploads
{
    public interface IUploadPathService
    {
        string BuildMemberAvatarFileKey(Guid memberId, string originalFileName);
        string ToPhysicalPath(string fileKey);
        string ToPublicUrl(string? fileKey);
    }
}
