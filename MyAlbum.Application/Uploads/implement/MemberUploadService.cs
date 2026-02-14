using MyAlbum.Application.Member;
using MyAlbum.Domain;
using MyAlbum.Models.UploadFiles;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Uploads.implement
{
    public class MemberUploadService : BaseService, IFileUploadService
    {
        public EntityUploadType entityUploadType => EntityUploadType.Member;

        private readonly IMemberAvatarUploadService _memberAvatarUploadService;
        public MemberUploadService(IAlbumDbContextFactory factory, IMemberAvatarUploadService memberAvatarUploadService) : base(factory)
        {
            _memberAvatarUploadService = memberAvatarUploadService;
        }

        public async Task<Dictionary<string, string>> Upload(UploadModel model, IReadOnlyList<UploadFileStream> files, CancellationToken ct = default)
        {
            // filename, path
            Dictionary<string, string> dict = new Dictionary<string, string>();

            if (model.ColumnType == 1)
            {
                foreach (var file in files)
                {
                    string path = await _memberAvatarUploadService.UploadAvatarAsync(model, file.Stream, file.FileName, ct);
                    dict.Add(file.FileName, path);
                }
            }
            return dict;
        }
    }
}
