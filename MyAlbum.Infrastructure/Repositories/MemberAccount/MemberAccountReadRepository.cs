using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.MemberAccount;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.Identity;
using MyAlbum.Models.MemberAccount;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Infrastructure.Repositories.MemberAccount
{
    public class MemberAccountReadRepository : IMemberAccountReadRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public MemberAccountReadRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<AccountDto?> GeMemberByUserNameAsync(GetMemAccountDto dto, CancellationToken ct = default)
        {
            var result = new AccountDto();
            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<MyAlbumContext>();

            var query = from mem in db.Members.AsNoTracking()
                        join account in db.Accounts.AsNoTracking() on mem.AccountId equals account.AccountId
                        where
                            account.AccountType == (int)AccountType.Member
                            && account.Status == (int)dto.AccountStatus
                            && mem.Status == (int)dto.MemStatus
                            && account.UserName == dto.UserName
                        select new AccountDto()
                        {
                            AccountId = account.AccountId,
                            UserName = account.UserName,
                            PasswordHash = account.PasswordHash,
                        };

            result = await query.FirstOrDefaultAsync(ct);

            return result;
        }
    }
}
