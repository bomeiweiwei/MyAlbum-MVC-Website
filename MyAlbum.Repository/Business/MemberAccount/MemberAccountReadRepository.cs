using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.MemberAccount;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.EmployeeAccount;
using MyAlbum.Models.Identity;
using MyAlbum.Models.MemberAccount;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository.Business.MemberAccount
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

        public async Task<MemberAccountDto?> GetMemberAccountAsync(GetMemberAccountReq req, CancellationToken ct = default)
        {
            var result = new MemberAccountDto();
            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<MyAlbumContext>();

            var query = from member in db.Members.AsNoTracking()
                        join account in db.Accounts.AsNoTracking() on member.AccountId equals account.AccountId
                        where
                            account.AccountType == (int)AccountType.Member
                            && member.MemberId == req.MemberId
                            && account.AccountId == req.AccountId
                        select new MemberAccountDto()
                        {
                            MemberId = member.MemberId,
                            AccountId = account.AccountId,
                            UserName = account.UserName,
                            Email = member.Email,
                            DisplayName = member.DisplayName,
                            Status = (Status)account.Status,
                        };
            result = await query.FirstOrDefaultAsync(ct);

            return result;
        }

        public async Task<List<MemberAccountDto>> GetMemberAccountListAsync(GetMemberAccountListReq req, CancellationToken ct = default)
        {
            var result = new List<MemberAccountDto>();
            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<MyAlbumContext>();

            var query = from member in db.Members.AsNoTracking()
                        join account in db.Accounts.AsNoTracking() on member.AccountId equals account.AccountId
                        where
                            account.AccountType == (int)AccountType.Member
                        select new MemberAccountDto()
                        {
                            MemberId = member.MemberId,
                            AccountId = account.AccountId,
                            UserName = account.UserName,
                            Email = member.Email,
                            DisplayName = member.DisplayName,
                            Status = (Status)account.Status,
                        };

            if (!string.IsNullOrWhiteSpace(req.UserName))
                query = query.Where(x => x.UserName.Contains(req.UserName));

            if (!string.IsNullOrWhiteSpace(req.DisplayName))
                query = query.Where(x => x.DisplayName.Contains(req.DisplayName));

            result = await query.ToListAsync(ct);

            return result;
        }
    }
}
