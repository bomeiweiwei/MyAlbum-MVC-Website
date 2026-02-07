using MyAlbum.Shared.Idenyity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Shared.Extensions
{
    public static class CurrentUserExtensions
    {
        public static Guid GetRequiredAccountId(this ICurrentUserAccessor accessor)
            => accessor.AccountId ?? throw new UnauthorizedAccessException();
    }
}
