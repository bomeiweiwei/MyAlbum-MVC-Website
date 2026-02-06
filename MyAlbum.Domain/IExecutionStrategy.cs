using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain
{
    public interface IExecutionStrategy
    {
        Task ExecuteAsync(Func<Task> operation, CancellationToken ct = default);
    }
}
