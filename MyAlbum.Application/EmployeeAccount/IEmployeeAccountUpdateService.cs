using MyAlbum.Models.EmployeeAccount;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.EmployeeAccount
{
    public interface IEmployeeAccountUpdateService
    {
        Task<bool> UpdateEmployeeAccountAsync(UpdateEmployeeAccountReq req, CancellationToken ct = default);

        Task<bool> UpdateEmployeeAccountActiveeAsync(UpdateEmployeeAccountActiveReq req, CancellationToken ct = default);
    }
}
