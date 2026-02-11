using MyAlbum.Domain;
using MyAlbum.Domain.AlbumPhoto;
using MyAlbum.Domain.Category;
using MyAlbum.Models.AlbumPhoto;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.AlbumPhoto.implement
{
    public class AlbumPhotoReadService : BaseService, IAlbumPhotoReadService
    {
        private readonly IAlbumPhotoReadRepository _albumPhotoReadRepository;
        public AlbumPhotoReadService(
            IAlbumDbContextFactory factory,
            IAlbumPhotoReadRepository albumPhotoReadRepository
            ) : base(factory)
        {
            _albumPhotoReadRepository = albumPhotoReadRepository;
        }
        public async Task<AlbumPhotoDto?> GetAlbumPhotoAsync(GetAlbumPhotoReq req, CancellationToken ct = default)
        {
            return await _albumPhotoReadRepository.GetAlbumPhotoAsync(req, ct);
        }

        public async Task<List<AlbumPhotoDto>> GetAlbumPhotoListAsync(GetAlbumPhotoReq req, CancellationToken ct = default)
        {
            return await _albumPhotoReadRepository.GetAlbumPhotoListAsync(req, ct);
        }
    }
}
