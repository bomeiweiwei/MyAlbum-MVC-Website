using MyAlbum.Models.EmployeeAccount;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.EmployeeAccount
{
    public interface IEmployeeAccountCreateService
    {
        Task<CreateEmployeeWithAccountResp> CreateEmployeeWithAccount(CreateEmployeeReq req, CancellationToken ct = default);
    }
}
