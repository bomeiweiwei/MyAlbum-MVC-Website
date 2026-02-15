using MyAlbum.Models.Base;
using MyAlbum.Models.EmployeeAccount;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.EmployeeAccount
{
    public interface IEmployeeAccountReadService
    {
        Task<EmployeeAccountDto?> GetEmployeeAccountAsync(GetEmployeeAccountReq req, CancellationToken ct = default);
        Task<ResponseBase<List<EmployeeAccountDto>>> GetEmployeeAccountListAsync(PageRequestBase<GetEmployeeAccountListReq> req, CancellationToken ct = default);
    }
}
