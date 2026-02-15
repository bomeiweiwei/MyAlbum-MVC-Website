using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Uploads
{
    public interface IUploadPathService
    {
        string BuildMemberAvatarFileKey(Guid memberId, string originalFileName);
        string BuildAlbumCoverPathFileKey(Guid albumId, string originalFileName);
        string BuildAlbumPhotoPathFileKey(Guid albumId, string originalFileName);
        string ToPhysicalPath(string fileKey);
        string ToPublicUrl(string? fileKey);
    }
}
