using MyAlbum.Application.Uploads;
using MyAlbum.Domain;
using MyAlbum.Domain.Album;
using MyAlbum.Domain.AlbumPhoto;
using MyAlbum.Domain.Category;
using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumPhoto.implement
{
    public class AlbumPhotoReadService : BaseService, IAlbumPhotoReadService
    {
        private readonly IAlbumPhotoReadRepository _albumPhotoReadRepository;
        private readonly IUploadPathService _paths;
        public AlbumPhotoReadService(
            IAlbumDbContextFactory factory,
            IAlbumPhotoReadRepository albumPhotoReadRepository,
            IUploadPathService paths) : base(factory)
        {
            _albumPhotoReadRepository = albumPhotoReadRepository;
            _paths = paths;
        }
        public async Task<AlbumPhotoDto?> GetAlbumPhotoAsync(GetAlbumPhotoReq req, CancellationToken ct = default)
        {
            var data = await _albumPhotoReadRepository.GetAlbumPhotoAsync(req, ct);
            if (data != null)
                data.PublicPathUrl = _paths.ToPublicUrl(data.PublicPathUrl);
            return data;
        }

        public async Task<ResponseBase<List<AlbumPhotoDto>>> GetAlbumPhotoListAsync(PageRequestBase<GetAlbumPhotoReq> req, CancellationToken ct = default)
        {
            var list = await _albumPhotoReadRepository.GetAlbumPhotoListAsync(req, ct);
            if (list.Data.Count > 0)
            {
                foreach (var data in list.Data)
                {
                    data.PublicPathUrl = _paths.ToPublicUrl(data.PublicPathUrl);
                }
            }
            return list;
        }
    }
}
