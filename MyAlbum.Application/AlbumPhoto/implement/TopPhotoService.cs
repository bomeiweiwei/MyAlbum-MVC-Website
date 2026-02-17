using MyAlbum.Application.Uploads;
using MyAlbum.Domain;
using MyAlbum.Domain.AlbumPhoto;
using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumPhoto.implement
{
    public class TopPhotoService : BaseService, ITopPhotoService
    {
        private readonly IAlbumPhotoReadRepository _read;
        private readonly IUploadPathService _paths;
        public TopPhotoService(
            IAlbumDbContextFactory factory,
            IAlbumPhotoReadRepository read,
            IUploadPathService paths) : base(factory)
        {
            _read = read;
            _paths = paths;
        }
        public async Task<List<AlbumPhotoDto>> GetTopPhotos(CancellationToken ct = default)
        {
            GetTopAlbumPhotoReq req = new GetTopAlbumPhotoReq
            {
                GetTopCount = 5,
                Status = Status.Active
            };
            var list = await _read.GetTopAlbumPhotoListAsync(req, ct);
            if (list.Count > 0)
            {
                foreach (var data in list)
                {
                    data.PublicPathUrl = _paths.ToPublicUrl(data.PublicPathUrl);
                }
            }
            return list;
        }
    }
}
