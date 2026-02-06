using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain
{
    public interface IAlbumDbContextFactory
    {
        IAlbumDbContext Create(ConnectionMode mode = ConnectionMode.Master);
    }
}
