using MyAlbum.Models.EmployeeAccount;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.EmployeeAccount
{
    public interface IEmployeeAccountReadService
    {
        Task<EmployeeAccountDto?> GetEmployeeAccountAsync(GetEmployeeAccountReq req, CancellationToken ct = default);
        Task<List<EmployeeAccountDto>> GetEmployeeAccountListAsync(GetEmployeeAccountListReq req, CancellationToken ct = default);
    }
}
