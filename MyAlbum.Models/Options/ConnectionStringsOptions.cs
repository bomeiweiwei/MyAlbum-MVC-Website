using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Options
{
    public sealed class ConnectionStringsOptions
    {
        public string MasterConnection { get; init; } = default!;
        public string SlaveConnection { get; init; } = default!;
    }
}
