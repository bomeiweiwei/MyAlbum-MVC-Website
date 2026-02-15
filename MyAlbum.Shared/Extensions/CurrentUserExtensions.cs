using MyAlbum.Shared.Enums;
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
        public static AccountType GetRequiredAccountType(this ICurrentUserAccessor accessor)
        {
            var value = accessor.GetClaim("AccountType");

            if (string.IsNullOrWhiteSpace(value))
                throw new UnauthorizedAccessException("AccountType claim not found.");

            if (!Enum.TryParse<AccountType>(value, ignoreCase: true, out var type))
                throw new UnauthorizedAccessException("Invalid AccountType claim.");

            return type;
        }
    }
}
