using MyAlbum.Domain;
using MyAlbum.Models.Account;
using MyAlbum.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Identity.implement
{
    public class LoginManagerService : BaseService, ILoginManagerService
    {
        private readonly IEnumerable<ILoginService> _loginServices;
        public LoginManagerService(IAlbumDbContextFactory factory,IEnumerable<ILoginService> loginServices) : base(factory)
        {
            _loginServices = loginServices;
        }
        /// <summary>
        /// 使用者登入
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<AccountLoginResp> UserLogin(LoginReq req, CancellationToken ct = default)
        {
            var result = new AccountLoginResp();
            var service = _loginServices.FirstOrDefault(x => x.userRole == req.Role);
            if (service == null)
            {
                throw new InvalidOperationException($"No login service found for type {req.Role}");
            }
            else
            {
                result = await service.Login(req, ct);
                return result;
            }
        }
    }
}
