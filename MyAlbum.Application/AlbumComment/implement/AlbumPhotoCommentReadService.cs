using MyAlbum.Application.AlbumPhoto;
using MyAlbum.Domain;
using MyAlbum.Models.AlbumComment;
using MyAlbum.Models.AlbumPhoto;
using MyAlbum.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumComment.implement
{
    public class AlbumPhotoCommentReadService : BaseService, IAlbumPhotoCommentReadService
    {
        private readonly IAlbumPhotoReadService _albumPhotoReadService;
        private readonly IAlbumCommentReadService _albumCommentReadService;
        public AlbumPhotoCommentReadService(
            IAlbumDbContextFactory factory,
            IAlbumPhotoReadService albumPhotoReadService,
            IAlbumCommentReadService albumCommentReadService
            ) : base(factory)
        {
            _albumPhotoReadService = albumPhotoReadService;
            _albumCommentReadService = albumCommentReadService;
        }

        public async Task<AlbumPhotoCommentsDto> GetAlbumPhotoComments(Guid Id)
        {
            var result = new AlbumPhotoCommentsDto();
            GetAlbumPhotoReq photoReq = new GetAlbumPhotoReq()
            {
                AlbumPhotoId = Id,
                Status = Shared.Enums.Status.Active
            };
            var photo = await _albumPhotoReadService.GetAlbumPhotoAsync(photoReq);
            if (photo == null)
                return result;
            result.Photo = photo;

            PageRequestBase<GetAlbumCommentListReq> pageReq = new PageRequestBase<GetAlbumCommentListReq>()
            {
                pageIndex = 1,
                pageSize = 100,
                Data = new GetAlbumCommentListReq()
                {
                    AlbumPhotoId = Id,
                    Status = Shared.Enums.Status.Active
                }
            };
            var comments = await _albumCommentReadService.GetAlbumCommentListAsync(pageReq);
            result.Comments = comments.Data;

            return result;
        }
    }
}
