using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.Account;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository.Business.Account
{
    public class AccountUpdateRepository : IAccountUpdateRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public AccountUpdateRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<bool> UpdateAccountAsync(IAlbumDbContext ctx, AccountUpdateDto accountDto, CancellationToken ct = default)
        {
            var result = false;

            var db = ctx.AsDbContext<MyAlbumContext>();
            var data = await db.Accounts.Where(x => x.AccountId == accountDto.AccountId).FirstOrDefaultAsync(ct);
            if (data == null)
                return result;

            if (!string.IsNullOrWhiteSpace(accountDto.PasswordHash))
            {
                data.PasswordHash = accountDto.PasswordHash;
            }

            data.Status = (byte)accountDto.Status;
            data.UpdatedBy = accountDto.UpdateBy;
            data.UpdatedAtUtc = DateTime.UtcNow;
            int check = await ctx.SaveChangesAsync(ct);
            if (check == 1)
                result = true;
            return result;
        }

        public async Task<bool> UpdateAccountActiveAsync(IAlbumDbContext ctx, AccountUpdateDto accountDto, CancellationToken ct = default)
        {
            var result = false;

            var db = ctx.AsDbContext<MyAlbumContext>();
            var data = await db.Accounts.Where(x => x.AccountId == accountDto.AccountId).FirstOrDefaultAsync(ct);
            if (data == null)
                return result;

            data.Status = (byte)accountDto.Status;
            data.UpdatedBy = accountDto.UpdateBy;
            data.UpdatedAtUtc = DateTime.UtcNow;
            int check = await ctx.SaveChangesAsync(ct);
            if (check == 1)
                result = true;
            return result;
        }
    }
}
