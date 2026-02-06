using MyAlbum.Domain;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application
{
    public class BaseService
    {
        private readonly IAlbumDbContextFactory _factory;
        protected BaseService(IAlbumDbContextFactory factory) => _factory = factory;

        protected IAlbumDbContext MainDB(ConnectionMode mode = ConnectionMode.Master) => _factory.Create(mode);
    }
}
