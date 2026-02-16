using MyAlbum.Application.Uploads;
using MyAlbum.Domain;
using MyAlbum.Domain.AlbumPhoto;
using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Models.Base;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.Idenyity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumPhoto.implement
{
    public class MemberAlbumPhotoReadService:BaseService, IMemberAlbumPhotoReadService
    {
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IAlbumPhotoReadRepository _read;
        private readonly IUploadPathService _paths;
        public MemberAlbumPhotoReadService(
            IAlbumDbContextFactory factory,
            ICurrentUserAccessor currentUser,
            IAlbumPhotoReadRepository read,
            IUploadPathService paths) : base(factory)
        {
            _read = read;
            _currentUser = currentUser;
            _paths = paths;
        }

        public async Task<AlbumPhotoDto?> GetAlbumPhotoAsync(GetAlbumPhotoReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();

            req.OwnerAccountId = operatorId;

            var data = await _read.GetAlbumPhotoAsync(req, ct);
            if (data != null)
                data.PublicPathUrl = _paths.ToPublicUrl(data.PublicPathUrl);
            return data;
        }

        public async Task<List<AlbumPhotoDto>> GetAlbumPhotoListAsync(GetAlbumPhotoReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();

            req.OwnerAccountId = operatorId;

            var pageReq = new PageRequestBase<GetAlbumPhotoReq>()
            {
                pageIndex = 1,
                pageSize = 99999,
                Data = req
            };
            var resp = await _read.GetAlbumPhotoListAsync(pageReq, ct);
            var list = resp.Data;
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
