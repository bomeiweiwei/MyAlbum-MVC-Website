using Microsoft.Extensions.Options;
using MyAlbum.Domain;
using MyAlbum.Models.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Uploads.implement
{
    public sealed class UploadPathService : BaseService, IUploadPathService
    {
        private readonly UploadOptions _opt;
        public UploadPathService(IAlbumDbContextFactory factory, IOptions<UploadOptions> opt) : base(factory) => _opt = opt.Value;

        public string BuildMemberAvatarFileKey(Guid memberId, string originalFileName)
        {
            var ext = NormalizeExt(Path.GetExtension(originalFileName));
            var fileName = $"{Guid.NewGuid():N}{ext}";
            return CombineKey(_opt.MemberImgRoot, memberId.ToString("N"), fileName);
        }

        public string ToPhysicalPath(string fileKey)
        {
            if (string.IsNullOrWhiteSpace(_opt.RootPath))
                throw new InvalidOperationException("Upload:RootPath 未設定");

            var relative = fileKey.Replace("/", Path.DirectorySeparatorChar.ToString());
            return Path.Combine(_opt.RootPath, relative);
        }

        public string ToPublicUrl(string? fileKey)
            => string.IsNullOrWhiteSpace(fileKey) ? string.Empty : "/uploads/" + fileKey.TrimStart('/');

        private static string CombineKey(params string[] parts)
            => string.Join("/", parts.Select(p => p.Trim('/').Replace("\\", "/")));

        private static string NormalizeExt(string ext)
        {
            ext = (ext ?? string.Empty).ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" => ".jpg",
                ".png" => ".png",
                ".webp" => ".webp",
                _ => ext
            };
        }
    }
}
