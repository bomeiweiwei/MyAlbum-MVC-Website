using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Infrastructure.EF.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository
{
    public sealed class EfExecutionStrategyFactory : IExecutionStrategyFactory
    {
        public IExecutionStrategy Create(IAlbumDbContext ctx)
        {
            var db = (ctx as EfAlbumDbContextAdapter)?.Db as MyAlbumContext
                     ?? throw new InvalidOperationException("Not EF context");

            var efStrategy = db.Database.CreateExecutionStrategy();
            return new StrategyWrapper(efStrategy);
        }

        private sealed class StrategyWrapper : IExecutionStrategy
        {
            private readonly Microsoft.EntityFrameworkCore.Storage.IExecutionStrategy _inner;
            public StrategyWrapper(Microsoft.EntityFrameworkCore.Storage.IExecutionStrategy inner) => _inner = inner;
            public Task ExecuteAsync(Func<Task> operation, CancellationToken ct = default)
                => _inner.ExecuteAsync(operation);
        }
    }
}
