using MyAlbum.Application.Uploads;
using MyAlbum.Domain;
using MyAlbum.Domain.Album;
using MyAlbum.Domain.Category;
using MyAlbum.Domain.MemberAccount;
using MyAlbum.Models.Album;
using MyAlbum.Models.Base;
using MyAlbum.Models.MemberAccount;
using MyAlbum.Shared.Enums;
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
            return data;
        }

        public async Task<ResponseBase<List<AlbumDto>>> GetAlbumListAsync(PageRequestBase<GetAlbumListReq> req, CancellationToken ct = default)
        {
            req.Data.StartReleaseTimeUtc = req.Data.StartLocalMs.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(req.Data.StartLocalMs.Value).UtcDateTime : null;
            req.Data.EndReleaseTimeUtc = req.Data.EndLocalMs.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(req.Data.EndLocalMs.Value).UtcDateTime : null;

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

        public async Task<List<AlbumDto>> GetAlbumListItemAsync(GetAlbumListReq req, CancellationToken ct = default)
        {
            var pageReq = new PageRequestBase<GetAlbumListReq>()
            {
                pageIndex = 1,
                pageSize = 99999,
                Data = req
            };
            //pageReq.Data.Status = Status.Active; // 由前端帶入
            var dataList = await GetAlbumListAsync(pageReq, ct);
            var list = dataList.Data;
            list = list.Select(m => new AlbumDto
            {
                AlbumId = m.AlbumId,
                Title = m.Title,
                Status = m.Status
            }).ToList();

            return list;
        }
    }
}
