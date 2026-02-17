using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MyAlbum.Shared.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? GetAccountId(this ClaimsPrincipal user)
        {
            var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }
}
