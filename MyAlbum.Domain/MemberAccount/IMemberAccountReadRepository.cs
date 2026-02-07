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
    }
}
