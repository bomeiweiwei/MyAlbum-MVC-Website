using MyAlbum.Models.EmployeeAccount;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.EmployeeAccount
{
    public interface IEmployeeAccountUpdateService
    {
        Task<UpdateResult> UpdateEmployeeAccountAsync(UpdateEmployeeAccountReq req, CancellationToken ct = default);

        Task<UpdateResult> UpdateEmployeeAccountActiveAsync(UpdateEmployeeAccountActiveReq req, CancellationToken ct = default);
    }
}
