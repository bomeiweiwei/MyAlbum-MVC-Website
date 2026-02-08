using MyAlbum.Models.EmployeeAccount;
using MyAlbum.Models.MemberAccount;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.MemberAccount
{
    public interface IMemberAccountUpdateService
    {
        Task<bool> UpdateMemberAccountAsync(UpdateMemberAccountReq req, CancellationToken ct = default);

        Task<bool> UpdateMemberAccountActiveAsync(UpdateMemberAccountActiveReq req, CancellationToken ct = default);
    }
}
