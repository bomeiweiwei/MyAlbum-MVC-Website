using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.EmployeeAccount;
using MyAlbum.Models.Identity;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Repository.Business.EmployeeAccount
{
    public class EmployeeAccountReadRepository : IEmployeeAccountReadRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public EmployeeAccountReadRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<AccountDto?> GetEmployeeByUserNameAsync(GetEmpAccountDto dto, CancellationToken ct = default)
        {
            var result = new AccountDto();
            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<MyAlbumContext>();

            var query = from emp in db.Employees.AsNoTracking()
                        join account in db.Accounts.AsNoTracking() on emp.AccountId equals account.AccountId
                        where
                            account.AccountType == (int)AccountType.Admin
                            && account.Status == (int)dto.AccountStatus
                            && emp.Status == (int)dto.EmpStatus
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

        public async Task<EmployeeAccountDto?> GetEmployeeAccountAsync(GetEmployeeAccountReq req, CancellationToken ct = default)
        {
            var result = new EmployeeAccountDto();
            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<MyAlbumContext>();

            var query = from emp in db.Employees.AsNoTracking()
                        join account in db.Accounts.AsNoTracking() on emp.AccountId equals account.AccountId
                        where
                            account.AccountType == (int)AccountType.Admin
                            && emp.EmployeeId == req.EmployeeId
                            && account.AccountId == req.AccountId
                        select new EmployeeAccountDto()
                        {
                            EmployeeId = emp.EmployeeId,
                            AccountId = account.AccountId,
                            UserName = account.UserName,
                            Email = emp.Email,
                            Phone = emp.Phone,
                            Status = (Status)account.Status,
                        };
            result = await query.FirstOrDefaultAsync(ct);

            return result;
        }

        public async Task<List<EmployeeAccountDto>> GetEmployeeAccountListAsync(GetEmployeeAccountListReq req, CancellationToken ct = default)
        {
            var result = new List<EmployeeAccountDto>();
            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<MyAlbumContext>();

            var query = from emp in db.Employees.AsNoTracking()
                        join account in db.Accounts.AsNoTracking() on emp.AccountId equals account.AccountId
                        where
                            account.AccountType == (int)AccountType.Admin
                        select new EmployeeAccountDto()
                        {
                            EmployeeId = emp.EmployeeId,
                            AccountId = account.AccountId,
                            UserName = account.UserName,
                            Email = emp.Email,
                            Phone = emp.Phone,
                            Status = (Status)account.Status,
                        };

            if (!string.IsNullOrWhiteSpace(req.UserName))
                query = query.Where(x => x.UserName.Contains(req.UserName));

            result = await query.ToListAsync(ct);

            return result;
        }

    }
}
