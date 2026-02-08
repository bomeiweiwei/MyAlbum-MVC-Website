using MyAlbum.Models.Account;
using MyAlbum.Models.Member;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Domain.MemberAccount
{
    public interface IMemberAccountCreateRepository
    {
        Task<Guid> CreateMemberWithAccountAsync(AccountCreateDto accountDto, MemberCreateDto employeeDto, CancellationToken ct = default);
    }
}
