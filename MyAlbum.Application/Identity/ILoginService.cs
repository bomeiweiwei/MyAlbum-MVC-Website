using MyAlbum.Models.Account;
using MyAlbum.Models.Identity;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Identity
{
    public interface ILoginService
    {
        UserRole userRole { get; }
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<AccountLoginResp> Login(LoginReq req, CancellationToken ct = default);
    }
}
