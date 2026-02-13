using MyAlbum.Application.Uploads;
using MyAlbum.Domain;
using MyAlbum.Domain.Album;
using MyAlbum.Domain.Category;
using MyAlbum.Domain.MemberAccount;
using MyAlbum.Models.Album;
using MyAlbum.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Album.implement
{
    public class AlbumReadService : BaseService, IAlbumReadService
    {
        private readonly IAlbumReadRepository _albumReadRepository;
        private readonly IUploadPathService _paths;
        public AlbumReadService(
            IAlbumDbContextFactory factory,
            IAlbumReadRepository albumReadRepository,
            IUploadPathService paths
            ) : base(factory)
        {
            _albumReadRepository = albumReadRepository;
            _paths = paths;
        }

        public async Task<AlbumDto?> GetAlbumAsync(GetAlbumReq req, CancellationToken ct = default)
        {
            var data = await _albumReadRepository.GetAlbumAsync(req, ct);
            if (data != null)
                data.PublicCoverUrl = _paths.ToPublicUrl(data.PublicCoverUrl);
            return await _albumReadRepository.GetAlbumAsync(req, ct);
        }

        public async Task<ResponseBase<List<AlbumDto>>> GetAlbumListAsync(PageRequestBase<GetAlbumListReq> req, CancellationToken ct = default)
        {
            var list = await _albumReadRepository.GetAlbumListAsync(req, ct);
            if (list.Data.Count > 0)
            {
                foreach (var data in list.Data)
                {
                    data.PublicCoverUrl = _paths.ToPublicUrl(data.PublicCoverUrl);
                }
            }
            return list;
        }
    }
}
