using MyAlbum.Models.EmployeeAccount;
using MyAlbum.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.EmployeeAccount
{
    public interface IEmployeeAccountReadRepository
    {
        /// <summary>
        /// 透過UserName取得員工資料
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<AccountDto?> GetEmployeeByUserNameAsync(GetEmpAccountDto dto, CancellationToken ct = default);

        Task<EmployeeAccountDto?> GetEmployeeAccountAsync(GetEmployeeAccountReq req, CancellationToken ct = default);

        Task<List<EmployeeAccountDto>> GetEmployeeAccountListAsync(GetEmployeeAccountListReq req, CancellationToken ct = default);
    }
}
