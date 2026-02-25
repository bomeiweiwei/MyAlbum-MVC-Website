using MyAlbum.Shared.Idenyity;
using System.Security.Claims;

namespace MyAlbum.Web.Identity
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? AccountId
        {
            get
            {
                var value = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.NameIdentifier)?
                    .Value;

                return Guid.TryParse(value, out var guid) ? guid : null;
            }
        }

        public string? UserName =>
            _httpContextAccessor.HttpContext?
                .User?
                .FindFirst(ClaimTypes.Name)?
                .Value;

        public string? AccountType =>
            _httpContextAccessor.HttpContext?
                .User?
                .FindFirst("AccountType")?
                .Value;

        public string? GetClaim(string claimType) => _httpContextAccessor.HttpContext?.User?.FindFirstValue(claimType);
    }
}
