using MyAlbum.Models.Base;
using MyAlbum.Models.EmployeeAccount;
using MyAlbum.Models.Identity;
using MyAlbum.Models.MemberAccount;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.MemberAccount
{
    public interface IMemberAccountReadRepository
    {
        Task<AccountDto?> GeMemberByUserNameAsync(GetMemAccountDto dto, CancellationToken ct = default);

        Task<MemberAccountDto?> GetMemberAccountAsync(GetMemberAccountReq req, CancellationToken ct = default);

        Task<ResponseBase<List<MemberAccountDto>>> GetMemberAccountListAsync(PageRequestBase<GetMemberAccountListReq> req, CancellationToken ct = default);
    }
}
