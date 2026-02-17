using MyAlbum.Application.Category;
using MyAlbum.Application.Uploads;
using MyAlbum.Domain;
using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Models.Base;
using MyAlbum.Models.Category;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumPhoto.implement
{
    public class CategoryPhotoReadService : BaseService, ICategoryPhotoReadService
    {
        private readonly IAlbumCategoryReadService _albumCategoryReadService;
        private readonly IAlbumPhotoReadService _albumPhotoReadService;
        private readonly IUploadPathService _paths;
        public CategoryPhotoReadService(
            IAlbumDbContextFactory factory,
            IAlbumCategoryReadService albumCategoryReadService,
            IAlbumPhotoReadService albumPhotoReadService,
            IUploadPathService paths
            ) : base(factory)
        {
            _albumCategoryReadService = albumCategoryReadService;
            _albumPhotoReadService = albumPhotoReadService;
            _paths = paths;
        }

        public async Task<AlbumCategoryViewDto> GetAlbumCategoryData(Guid Id, CancellationToken ct = default)
        {
            var result = new AlbumCategoryViewDto();

            var categoryReq = new GetAlbumCategoryReq 
            { 
                AlbumCategoryId = Id ,
                Status = Shared.Enums.Status.Active
            };
            var category = await _albumCategoryReadService.GetAlbumCategoryAsync(categoryReq, ct);
            if (category == null)
            {
                return result;
            }
            result.CategoryName = category.CategoryName;

            PageRequestBase<GetAlbumPhotoReq> pageReq = new PageRequestBase<GetAlbumPhotoReq>()
            {
                pageIndex = 1,
                pageSize = 99999,
                Data = new GetAlbumPhotoReq()
                {
                    AlbumCategoryId = Id,
                    Status = Shared.Enums.Status.Active
                }
            };

            var photos = await _albumPhotoReadService.GetAlbumPhotoListAsync(pageReq);
            result.Photos = photos.Data;
            return result;
        }
    }
}
