using MyAlbum.Application.Uploads;
using MyAlbum.Domain;
using MyAlbum.Domain.Album;
using MyAlbum.Models.Album;
using MyAlbum.Models.Base;
using MyAlbum.Shared.Extensions;
using MyAlbum.Shared.Idenyity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Album.implement
{
    public class MemberAlbumReadService : BaseService, IMemberAlbumReadService
    {
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IAlbumReadRepository _read;
        private readonly IUploadPathService _paths;
        public MemberAlbumReadService(
            IAlbumDbContextFactory factory,
            ICurrentUserAccessor currentUser,
            IUploadPathService paths,
            IAlbumReadRepository read
          ) : base(factory)
        {
            _currentUser = currentUser;
            _paths = paths;
            _read = read;
        }
        public async Task<List<AlbumDto>> GetAlbumListAsync(GetAlbumListReq req, CancellationToken ct = default)
        {
            var operatorId = _currentUser.GetRequiredAccountId();

            req.OwnerAccountId = operatorId;
            
            PageRequestBase<GetAlbumListReq> pageReq=new PageRequestBase<GetAlbumListReq>()
            {
                pageIndex = 1,
                pageSize = 99999,
                Data = req
            };
            var resp = await _read.GetAlbumListAsync(pageReq, ct);
            var list = resp.Data;
            if (list.Count > 0)
            {
                foreach (var data in list)
                {
                    data.PublicCoverUrl = _paths.ToPublicUrl(data.PublicCoverUrl);
                }
            }
            return list;
        }
    }
}
