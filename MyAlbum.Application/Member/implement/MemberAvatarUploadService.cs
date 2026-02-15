using MyAlbum.Application.Uploads;
using MyAlbum.Domain;
using MyAlbum.Domain.Member;
using MyAlbum.Models.UploadFiles;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.Idenyity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Member.implement
{
    public class MemberAvatarUploadService : BaseService, IMemberAvatarUploadService
    {
        private readonly IUploadPathService _paths;
        public MemberAvatarUploadService(
           IAlbumDbContextFactory factory,
           IUploadPathService paths
           ) : base(factory)
        {
            _paths = paths;
        }

        public async Task<string> UploadAvatarAsync(UploadModel model, Stream fileStream, string originalFileName, CancellationToken ct)
        {
            var fileKey = _paths.BuildMemberAvatarFileKey(model.Id, originalFileName);

            var physicalPath = _paths.ToPhysicalPath(fileKey);
            Directory.CreateDirectory(Path.GetDirectoryName(physicalPath)!);

            try
            {
                await using (var outStream = File.Create(physicalPath))
                {
                    await fileStream.CopyToAsync(outStream, ct);
                }

                //return _paths.ToPublicUrl(fileKey);
                return fileKey;
            }
            catch
            {
                try { if (File.Exists(physicalPath)) File.Delete(physicalPath); } catch { /* ignore */ }
                throw;
            }
        }
    }
}
