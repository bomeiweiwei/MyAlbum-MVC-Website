using MyAlbum.Application.Uploads;
using MyAlbum.Domain;
using MyAlbum.Domain.Member;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.Idenyity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Member.implement
{
    public class MemberDataUploadService : BaseService, IMemberDataUploadService
    {
        private readonly IUploadPathService _paths;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IMemberUpdateRepository _memberUpdateRepository;
        public MemberDataUploadService(
           IAlbumDbContextFactory factory,
           IUploadPathService paths,
           ICurrentUserAccessor currentUser,
           IMemberUpdateRepository memberUpdateRepository
           ) : base(factory)
        {
            _currentUser = currentUser;
            _paths = paths;
            _memberUpdateRepository = memberUpdateRepository;
        }

        public async Task<string> UploadAvatarAsync(Guid memberId, Stream fileStream, string originalFileName, CancellationToken ct)
        {
            var id = _currentUser.GetRequiredAccountId();
            var fileKey = _paths.BuildMemberAvatarFileKey(memberId, originalFileName);

            var physicalPath = _paths.ToPhysicalPath(fileKey);
            Directory.CreateDirectory(Path.GetDirectoryName(physicalPath)!);

            try
            {
                await using (var outStream = File.Create(physicalPath))
                {
                    await fileStream.CopyToAsync(outStream, ct);
                }

                await _memberUpdateRepository.UpdateMemberAvatarPathAsync(memberId, fileKey, id, ct);

                return _paths.ToPublicUrl(fileKey);
            }
            catch
            {
                try { if (File.Exists(physicalPath)) File.Delete(physicalPath); } catch { /* ignore */ }
                throw;
            }
        }
    }
}
