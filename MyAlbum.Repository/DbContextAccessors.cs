using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository
{
    internal static class DbContextAccessors
    {
        internal static T AsDbContext<T>(this IAlbumDbContext ctx) where T : DbContext
        {
            if (ctx is EfAlbumDbContextAdapter ef) return (T)ef.Db;
            throw new InvalidOperationException("IAlbumDbContext 不是 EF 的實作。");
        }
    }
}
