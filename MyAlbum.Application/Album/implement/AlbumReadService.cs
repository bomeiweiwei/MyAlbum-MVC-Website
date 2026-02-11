using MyAlbum.Domain;
using MyAlbum.Domain.Album;
using MyAlbum.Domain.Category;
using MyAlbum.Models.Album;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Album.implement
{
    public class AlbumReadService : BaseService, IAlbumReadService
    {
        private readonly IAlbumReadRepository _albumReadRepository;
        public AlbumReadService(
            IAlbumDbContextFactory factory,
            IAlbumReadRepository albumReadRepository
            ) : base(factory)
        {
            _albumReadRepository = albumReadRepository;
        }

        public async Task<AlbumDto?> GetAlbumAsync(GetAlbumReq req, CancellationToken ct = default)
        {
            return await _albumReadRepository.GetAlbumAsync(req, ct);
        }

        public async Task<List<AlbumDto>> GetAlbumListAsync(GetAlbumReq req, CancellationToken ct = default)
        {
            return await _albumReadRepository.GetAlbumListAsync(req, ct);
        }
    }
}
