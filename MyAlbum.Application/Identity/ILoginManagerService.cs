using MyAlbum.Models.Account;
using MyAlbum.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Identity
{
    /// <summary>
    /// 統一對外提供的「登入入口」
    /// </summary>
    public interface ILoginManagerService
    {
        /// <summary>
        /// 使用者登入
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<AccountLoginResp> UserLogin(LoginReq req, CancellationToken ct = default);
    }
}
