using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyAlbum.Domain;
using MyAlbum.Infrastructure;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.Options;
using MyAlbum.Repository.Extensions;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository
{
    public sealed class AlbumDbContextFactory : IAlbumDbContextFactory
    {
        private readonly ConnectionStringsOptions _conn;
        public AlbumDbContextFactory(IOptions<ConnectionStringsOptions> options)
        {
            _conn = options.Value;
        }

        public IAlbumDbContext Create(ConnectionMode mode = ConnectionMode.Master)
        {
            var options = new DbContextOptionsBuilder<MyAlbumContext>();

            var cs = mode == ConnectionMode.Master
                ? _conn.MasterConnection
                : _conn.SlaveConnection;

            options.OptionsBuilderSetting(cs); // 你既有的擴充/設定

            var ctx = new MyAlbumContext(options.Options);
            return new EfAlbumDbContextAdapter(ctx);
        }
    }
}
