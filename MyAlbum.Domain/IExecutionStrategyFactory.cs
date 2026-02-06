using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain
{
    public interface IExecutionStrategyFactory
    {
        IExecutionStrategy Create(IAlbumDbContext ctx);
    }
}
