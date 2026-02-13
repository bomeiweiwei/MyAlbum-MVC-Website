using MyAlbum.Domain;
using MyAlbum.Domain.Category;
using MyAlbum.Models.Base;
using MyAlbum.Models.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Category.implement
{
    public class AlbumCategoryReadService : BaseService, IAlbumCategoryReadService
    {
        private readonly IAlbumCategoryReadRepository _albumCategoryReadRepository;
        public AlbumCategoryReadService(
            IAlbumDbContextFactory factory,
            IAlbumCategoryReadRepository albumCategoryReadRepository
            ) : base(factory)
        {
            _albumCategoryReadRepository = albumCategoryReadRepository;
        }

        public async Task<AlbumCategoryDto?> GetAlbumCategoryAsync(GetAlbumCategoryReq req, CancellationToken ct = default)
        {
            return await _albumCategoryReadRepository.GetAlbumCategoryAsync(req, ct);
        }

        public async Task<ResponseBase<List<AlbumCategoryDto>>> GetAlbumCategoryListAsync(PageRequestBase<GetAlbumCategoryListReq> req, CancellationToken ct = default)
        {
            return await _albumCategoryReadRepository.GetAlbumCategoryListAsync(req, ct);
        }

        public async Task<List<AlbumCategoryDto>> GetAlbumCategoryItemListAsync(GetAlbumCategoryListReq req, CancellationToken ct = default)
        {
            req.Status = Shared.Enums.Status.Active;
            var pageReq = new PageRequestBase<GetAlbumCategoryListReq>()
            {
                pageIndex = 1,
                pageSize = 99999,
                Data = req
            };
            var dataList = await GetAlbumCategoryListAsync(pageReq);

            var list = dataList.Data;
            list = list.Select(m => new AlbumCategoryDto
            {
                AlbumCategoryId = m.AlbumCategoryId,
                CategoryName = m.CategoryName,
                Status = m.Status
            }).ToList();

            return list;
        }
    }
}
