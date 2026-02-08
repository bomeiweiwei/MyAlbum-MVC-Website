using MyAlbum.Models.EmployeeAccount;
using MyAlbum.Models.MemberAccount;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.MemberAccount
{
    public interface IMemberAccountReadService
    {
        Task<MemberAccountDto?> GetMemberAccountAsync(GetMemberAccountReq req, CancellationToken ct = default);

        Task<List<MemberAccountDto>> GetMemberAccountListAsync(GetMemberAccountListReq req, CancellationToken ct = default);
    }
}
